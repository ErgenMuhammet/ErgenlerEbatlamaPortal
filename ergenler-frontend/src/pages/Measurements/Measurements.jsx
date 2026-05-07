import { useState, useEffect } from 'react';
import { orderService } from '../../services/api';
import './Measurements.css';
import { FiSearch, FiPackage, FiLayers, FiMinimize2 } from 'react-icons/fi';

export default function Measurements() {
  const [orders, setOrders] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [selectedOrder, setSelectedOrder] = useState(null);
  const [measurements, setMeasurements] = useState(null);
  const [loadingList, setLoadingList] = useState(false);
  const [loadingData, setLoadingData] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    loadOrderList();
  }, []);

  const loadOrderList = async () => {
    setLoadingList(true);
    try {
      const res = await orderService.getAllOrdersList();
      if (res.data?.isSuccess) {
        setOrders(res.data.fileDetails || []);
      }
    } catch (err) {
      console.error(err);
    } finally {
      setLoadingList(false);
    }
  };

  const handleSelectOrder = async (order) => {
    setSelectedOrder(order);
    setSearchTerm(order.fileName);
    setIsDropdownOpen(false);
    
    setLoadingData(true);
    setError('');
    setMeasurements(null);
    try {
      const res = await orderService.getMeasurements({ path: order.filePath, orderName: order.fileName });
      if (res.data?.isSuccess) {
        setMeasurements(res.data);
      } else {
        setError(res.data?.message || 'Ölçüler alınamadı.');
      }
    } catch (err) {
      setError('Ölçüler alınırken bir hata oluştu.');
      console.error(err);
    } finally {
      setLoadingData(false);
    }
  };

  const filteredOrders = orders.filter((o) =>
    o.fileName?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="measurements-page">
      <div className="search-section">
        <h2 className="section-title">Sipariş Seçimi</h2>
        <div className="search-container">
          <div className="search-input-wrapper">
            <FiSearch className="search-icon" />
            <input
              type="text"
              className="form-input search-input"
              placeholder="Klasör / Sipariş adı ara..."
              value={searchTerm}
              onChange={(e) => {
                setSearchTerm(e.target.value);
                setIsDropdownOpen(true);
              }}
              onFocus={() => setIsDropdownOpen(true)}
            />
          </div>
          
          {isDropdownOpen && (
            <div className="search-dropdown slide-up">
              {loadingList ? (
                <div className="dropdown-item text-muted">Yükleniyor...</div>
              ) : filteredOrders.length > 0 ? (
                filteredOrders.map((order, idx) => (
                  <div
                    key={idx}
                    className="dropdown-item"
                    onClick={() => handleSelectOrder(order)}
                  >
                    {order.fileName}
                  </div>
                ))
              ) : (
                <div className="dropdown-item text-danger">Sipariş bulunamadı</div>
              )}
            </div>
          )}
        </div>
      </div>

      {error && <div className="error-alert">{error}</div>}

      {loadingData ? (
        <div className="loading-container">
          <div className="loading-spinner" />
        </div>
      ) : measurements ? (
        <div className="results-section fade-in">
          <div className="results-header">
            <h3>{measurements.orders?.customerName || selectedOrder?.fileName}</h3>
            <span className="results-subtitle">Ebatlama Detayları</span>
          </div>

          <div className="stats-grid">
            <div className="stat-card">
              <div className="stat-icon"><FiLayers /></div>
              <div className="stat-info">
                <span className="stat-label">Kullanılan MDF (Plaka)</span>
                {/* As an example, if there is a calculation, or just display the length. Based on data available */}
                <span className="stat-value">{measurements.mdfProporties?.length || 0} Parça</span>
              </div>
            </div>
            <div className="stat-card">
              <div className="stat-icon"><FiMinimize2 /></div>
              <div className="stat-info">
                <span className="stat-label">Kullanılan Bant (Metre)</span>
                <span className="stat-value">Detaylarda</span>
              </div>
            </div>
          </div>

          <div className="table-container">
            <h4>Kesim Ölçüleri (MDF Özellikleri)</h4>
            <table className="data-table">
              <thead>
                <tr>
                  <th>Yükseklik</th>
                  <th>Genişlik</th>
                  <th>Kalınlık (MM)</th>
                  <th>Adet</th>
                </tr>
              </thead>
              <tbody>
                {measurements.mdfProporties?.map((prop, idx) => (
                  <tr key={idx}>
                    <td>{prop.height} mm</td>
                    <td>{prop.width} mm</td>
                    <td>{prop.mm} mm</td>
                    <td>{prop.count}</td>
                  </tr>
                ))}
                {!measurements.mdfProporties?.length && (
                  <tr>
                    <td colSpan="4" className="text-center">Ölçü bulunamadı.</td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
          
          {measurements.orders?.sawnPiece && (
             <div className="table-container">
             <h4>Sipariş Parçaları</h4>
             <table className="data-table">
               <thead>
                 <tr>
                   <th>Parça Adı</th>
                   {/* Add more headers based on SawnPieceForOrders fields if known */}
                 </tr>
               </thead>
               <tbody>
                 {measurements.orders.sawnPiece.map((piece, idx) => (
                   <tr key={idx}>
                     <td>{piece.name || 'Parça ' + (idx + 1)}</td>
                   </tr>
                 ))}
               </tbody>
             </table>
           </div>
          )}
        </div>
      ) : (
        <div className="empty-state">
          <div className="empty-state-icon"><FiPackage /></div>
          <p>Detayları görmek için yukarıdan bir sipariş klasörü seçin.</p>
        </div>
      )}
    </div>
  );
}
