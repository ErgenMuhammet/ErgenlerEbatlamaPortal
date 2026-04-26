import { useState, useEffect } from 'react';
import { materialService } from '../../services/api';
import { FiPlus, FiEdit2, FiPackage, FiX } from 'react-icons/fi';
import './Stock.css';

const tabs = [
  { key: 'mdf', label: 'MDF' },
  { key: 'pvcBand', label: 'PVC Bant' },
  { key: 'glue', label: 'Tutkal' },
  { key: 'backPanel', label: 'Arka Panel' },
  { key: 'scraps', label: 'Kırpıntı' },
];

const popularBrands = ['Yıldız Entegre', 'Kastamonu Entegre', 'Çamsan', 'AGT', 'Starwood', 'Kronospan', 'Divapan', 'Diğer'];
const popularThicknesses = ['3', '6', '8', '10', '12', '16', '18', '22', '25', '30'];

const fetchMap = {
  mdf: materialService.getMdfStock,
  pvcBand: materialService.getPvcBandStock,
  glue: materialService.getGlueStock,
  backPanel: materialService.getBackPanelStock,
  scraps: materialService.getScraps,
};

const columnMap = {
  mdf: ['brand', 'color', 'thickness', 'stock', 'cost', 'price'],
  pvcBand: ['brand', 'color', 'stock', 'cost', 'price'],
  glue: ['brand', 'stock', 'cost', 'price'],
  backPanel: ['brand', 'stock', 'cost', 'price'],
  scraps: ['brand', 'color', 'stock', 'cost', 'price'],
};

const columnLabels = {
  brand: 'Marka',
  color: 'Renk',
  thickness: 'Kalınlık',
  stock: 'Stok',
  cost: 'Maliyet (₺)',
  price: 'Fiyat (₺)',
};

export default function Stock() {
  const [activeTab, setActiveTab] = useState('mdf');
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState({});

  useEffect(() => {
    loadData();
  }, [activeTab]);

  const loadData = async () => {
    setLoading(true);
    try {
      const res = await fetchMap[activeTab]();
      setData(res.data?.data || res.data || []);
    } catch {
      setData([]);
    } finally {
      setLoading(false);
    }
  };

  const handleAdd = async () => {
    try {
      const addMap = {
        mdf: materialService.addMdf,
        pvcBand: materialService.addPvcBand,
        glue: materialService.addGlue,
        backPanel: materialService.addBackPanel,
        scraps: materialService.addScrap,
      };
      await addMap[activeTab](formData);
      setShowModal(false);
      setFormData({});
      loadData();
    } catch (err) {
      console.error('Ekleme hatası:', err);
    }
  };

  const columns = columnMap[activeTab];

  const getStockBadge = (stock) => {
    if (stock <= 0) return <span className="badge badge-danger">Tükendi</span>;
    if (stock < 10) return <span className="badge badge-warning">Düşük</span>;
    return <span className="badge badge-success">Yeterli</span>;
  };

  return (
    <div className="stock-page">
      {/* Tabs */}
      <div className="stock-tabs">
        {tabs.map((tab) => (
          <button
            key={tab.key}
            className={`stock-tab ${activeTab === tab.key ? 'active' : ''}`}
            onClick={() => setActiveTab(tab.key)}
          >
            {tab.label}
          </button>
        ))}
      </div>

      {/* Table */}
      <div className="stock-table-wrapper">
        <div className="stock-table-header">
          <span className="stock-table-title">
            {tabs.find((t) => t.key === activeTab)?.label} Stok Listesi
          </span>
          <button className="add-btn" onClick={() => { setFormData({}); setShowModal(true); }}>
            <FiPlus /> Yeni Ekle
          </button>
        </div>

        {loading ? (
          <div className="loading-container">
            <div className="loading-spinner" />
          </div>
        ) : data.length === 0 ? (
          <div className="empty-state">
            <div className="empty-state-icon"><FiPackage /></div>
            <p>Henüz kayıt bulunmuyor</p>
          </div>
        ) : (
          <table className="data-table">
            <thead>
              <tr>
                {columns.map((col) => (
                  <th key={col}>{columnLabels[col]}</th>
                ))}
                <th>Durum</th>
                <th>İşlem</th>
              </tr>
            </thead>
            <tbody>
              {data.map((item, idx) => (
                <tr key={item.id || idx}>
                  {columns.map((col) => (
                    <td key={col}>
                      {col === 'cost' || col === 'price'
                        ? `₺${(item[col] || 0).toLocaleString('tr-TR')}`
                        : item[col] ?? '-'}
                    </td>
                  ))}
                  <td>{getStockBadge(item.stock)}</td>
                  <td>
                    <button className="table-action-btn edit">
                      <FiEdit2 />
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>

      {/* Add Modal */}
      {showModal && (
        <div className="modal-overlay" onClick={() => setShowModal(false)}>
          <div className="modal" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h3>{tabs.find((t) => t.key === activeTab)?.label} Ekle</h3>
              <button className="modal-close" onClick={() => setShowModal(false)}>
                <FiX />
              </button>
            </div>
            <div className="modal-body">
              {columns.map((col) => {
                if (col === 'brand') {
                  return (
                    <div className="form-group" key={col}>
                      <label className="form-label">{columnLabels[col]}</label>
                      <select
                        className="form-input"
                        value={formData[col] || ''}
                        onChange={(e) => setFormData({ ...formData, [col]: e.target.value })}
                      >
                        <option value="">Seçiniz...</option>
                        {popularBrands.map((brand) => (
                          <option key={brand} value={brand}>{brand}</option>
                        ))}
                      </select>
                    </div>
                  );
                }
                
                if (col === 'thickness') {
                  return (
                    <div className="form-group" key={col}>
                      <label className="form-label">{columnLabels[col]} (mm)</label>
                      <select
                        className="form-input"
                        value={formData[col] || ''}
                        onChange={(e) => setFormData({ ...formData, [col]: e.target.value })}
                      >
                        <option value="">Seçiniz...</option>
                        {popularThicknesses.map((t) => (
                          <option key={t} value={t}>{t} mm</option>
                        ))}
                      </select>
                    </div>
                  );
                }

                return (
                  <div className="form-group" key={col}>
                    <label className="form-label">{columnLabels[col]}</label>
                    <input
                      className="form-input"
                      type={col === 'color' ? 'text' : 'number'}
                      placeholder={columnLabels[col]}
                      value={formData[col] || ''}
                      onChange={(e) => setFormData({ ...formData, [col]: e.target.value })}
                    />
                  </div>
                );
              })}
            </div>
            <div className="modal-footer">
              <button className="btn-cancel" onClick={() => setShowModal(false)}>İptal</button>
              <button className="btn-save" onClick={handleAdd}>Kaydet</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
