import { useState, useEffect } from 'react';
import { materialService } from '../../services/api';
import { FiPlus, FiEdit2, FiPackage, FiX, FiMinus } from 'react-icons/fi';
import './Stock.css';

const tabs = [
  { key: 'mdf', label: 'MDF' },
  { key: 'pvcBand', label: 'PVC Bant' },
  { key: 'glue', label: 'Tutkal' },
  { key: 'backPanel', label: 'Arka Panel' },
  { key: 'scraps', label: 'Fireler' },
];

const popularBrands = [
  'Yıldız Entegre', 'Kastamonu Entegre', 'Çamsan', 'AGT', 'Starwood', 
  'Kronospan', 'Divapan', 'Gentaş', 'Teverpan', 'SFC Entegre', 'Pelit Arslan',
  'Roma Plastik', 'Tece', 'Portakal', 'Apel', 'Akfix', 'Mitreapel', 'Selsil', 
  'Diğer'
];
const popularThicknesses = ['3', '6', '8', '10', '12', '16', '18', '22', '25', '30'];
const popularWeights = Array.from({ length: 30 }, (_, i) => String(64 + i * 2));
const popularColors = [
  'Beyaz', 'Siyah', 'Antrasit', 'Gri', 'Metalik Gri',
  'Krem', 'Aytaşı', 'Keten', 'Ceviz', 'Milano Ceviz', 'Meşe', 'Safir Meşe', 
  'Sonoma Meşe', 'Kordoba', 'Venge', 'Abanoz', 'Teak', 'Huş', 'Akçaağaç', 'Kiraz', 'Bambu',
  'Diğer'
];

const fetchMap = {
  mdf: materialService.getMdfStock,
  pvcBand: materialService.getPvcBandStock,
  glue: materialService.getGlueStock,
  backPanel: materialService.getBackPanelStock,
  scraps: materialService.getScraps,
};

const columnMap = {
  mdf: ['brand', 'color', 'thickness', 'weight', 'stock'],
  pvcBand: ['brand', 'color', 'thickness', 'stock'],
  glue: ['brand', 'weight', 'stock'],
  backPanel: ['brand', 'color', 'thickness', 'stock'],
  scraps: ['materialType', 'color', 'width', 'height', 'stock'],
};

const columnLabels = {
  brand: 'Marka',
  color: 'Renk',
  thickness: 'Kalınlık',
  weight: 'Ağırlık',
  stock: 'Stok',
  width: 'En (cm)',
  height: 'Boy (cm)',
  materialType: 'Tür',
};

export default function Stock() {
  const [activeTab, setActiveTab] = useState('mdf');
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState({});
  const [editingItem, setEditingItem] = useState(null);
  const [showReduceModal, setShowReduceModal] = useState(false);
  const [reduceItem, setReduceItem] = useState(null);
  const [reduceCount, setReduceCount] = useState('');

  useEffect(() => {
    loadData();
  }, [activeTab]);

  const loadData = async () => {
    setLoading(true);
    try {
      const res = await fetchMap[activeTab]();
      const responseData = res.data;
      
      let arr = [];
      if (Array.isArray(responseData)) {
        arr = responseData;
      } else if (responseData && typeof responseData === 'object') {
        if (Array.isArray(responseData.data)) {
          arr = responseData.data;
        } else {
          // Find the first property that is an array (e.g. res.data.mdf)
          for (const key in responseData) {
            if (Array.isArray(responseData[key])) {
              arr = responseData[key];
              break;
            }
          }
        }
      }
      
      // Sadece stoku 0'dan büyük olanları göster
      arr = arr.filter(item => item.stock > 0);
      
      setData(arr);
    } catch {
      setData([]);
    } finally {
      setLoading(false);
    }
  };

  const handleAdd = async () => {
    try {
      const payload = { ...formData };
      ['brand', 'thickness', 'weight', 'color'].forEach(col => {
        if (payload[col] === 'Diğer' && payload[`${col}Custom`]) {
          const customVal = payload[`${col}Custom`];
          payload[col] = (col === 'brand' || col === 'color') ? customVal.toLowerCase() : parseFloat(customVal);
        }
        delete payload[`${col}Custom`];
      });

      // Backend API için sayısal değerleri doğru tipe dönüştürüyoruz
      if ('stock' in payload) payload.stock = parseInt(payload.stock, 10) || 0;
      if ('thickness' in payload && typeof payload.thickness === 'string') payload.thickness = parseFloat(payload.thickness) || 0;
      if ('weight' in payload && typeof payload.weight === 'string') payload.weight = parseInt(payload.weight, 10) || parseFloat(payload.weight) || 0;
      if ('width' in payload && typeof payload.width === 'string') payload.width = parseFloat(payload.width) || 0;
      if ('height' in payload && typeof payload.height === 'string') payload.height = parseFloat(payload.height) || 0;

      if (editingItem) {
        const updateMap = {
          mdf: materialService.updateMdf,
          pvcBand: materialService.updatePvcBand,
          glue: materialService.updateGlue,
          backPanel: materialService.updateBackPanel,
          scraps: materialService.updateScrap,
        };
        await updateMap[activeTab](editingItem.id, payload);
      } else {
        const addMap = {
          mdf: materialService.addMdf,
          pvcBand: materialService.addPvcBand,
          glue: materialService.addGlue,
          backPanel: materialService.addBackPanel,
          scraps: materialService.addScrap,
        };
        await addMap[activeTab](payload);
      }
      setShowModal(false);
      setFormData({});
      setEditingItem(null);
      loadData();
    } catch (err) {
      console.error('İşlem hatası:', err);
    }
  };

  const handleReduce = async () => {
    try {
      const count = parseInt(reduceCount, 10);
      if (!count || count <= 0) return;
      
      const payload = { count };

      const reduceMap = {
        mdf: materialService.reduceMdf,
        pvcBand: materialService.reducePvcBand,
        glue: materialService.reduceGlue,
        backPanel: materialService.reduceBackPanel,
        scraps: materialService.reduceScrap,
      };

      await reduceMap[activeTab](reduceItem.id, payload);
      setShowReduceModal(false);
      setReduceItem(null);
      setReduceCount('');
      loadData();
    } catch (err) {
      console.error('Eksiltme hatası:', err);
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
          <button className="add-btn" onClick={() => { setEditingItem(null); setFormData({}); setShowModal(true); }}>
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
                <tr key={item.id || item.Id || idx}>
                  {columns.map((col) => (
                    <td key={col}>
                      {item[col] ?? item[col.charAt(0).toUpperCase() + col.slice(1)] ?? '-'}
                    </td>
                  ))}
                  <td>{getStockBadge(item.stock ?? item.Stock)}</td>
                  <td style={{ display: 'flex', gap: '0.5rem' }}>
                    <button 
                      className="table-action-btn edit"
                      title="Düzenle"
                      onClick={() => {
                        const newFormData = { ...item };
                        if (item.brand && !popularBrands.some(b => b.toLowerCase() === item.brand.toLowerCase())) {
                           newFormData.brand = 'Diğer';
                           newFormData.brandCustom = item.brand;
                        }
                        if (item.color && !popularColors.some(c => c.toLowerCase() === item.color.toLowerCase())) {
                           newFormData.color = 'Diğer';
                           newFormData.colorCustom = item.color;
                        }
                        if (item.thickness !== undefined && !popularThicknesses.includes(String(item.thickness))) {
                           newFormData.thickness = 'Diğer';
                           newFormData.thicknessCustom = item.thickness;
                        }
                        if (item.weight !== undefined && !popularWeights.includes(String(item.weight))) {
                           newFormData.weight = 'Diğer';
                           newFormData.weightCustom = item.weight;
                        }
                        setEditingItem(item);
                        setFormData(newFormData);
                        setShowModal(true);
                      }}
                    >
                      <FiEdit2 />
                    </button>
                    <button 
                      className="table-action-btn"
                      title="Stok Azalt"
                      style={{ color: 'var(--danger-color)', backgroundColor: 'rgba(239, 68, 68, 0.1)' }}
                      onClick={() => {
                        setReduceItem(item);
                        setReduceCount('');
                        setShowReduceModal(true);
                      }}
                    >
                      <FiMinus />
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
              <h3>{tabs.find((t) => t.key === activeTab)?.label} {editingItem ? 'Düzenle' : 'Ekle'}</h3>
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
                      {formData[col] === 'Diğer' && (
                        <input
                          className="form-input"
                          type="text"
                          style={{ marginTop: '0.5rem' }}
                          placeholder="Marka adını giriniz"
                          value={formData[`${col}Custom`] || ''}
                          onChange={(e) => setFormData({ ...formData, [`${col}Custom`]: e.target.value })}
                        />
                      )}
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
                        onChange={(e) => setFormData({ ...formData, [col]: e.target.value === 'Diğer' ? 'Diğer' : parseFloat(e.target.value) || '' })}
                      >
                        <option value="">Seçiniz...</option>
                        {popularThicknesses.map((t) => (
                          <option key={t} value={t}>{t} mm</option>
                        ))}
                        <option value="Diğer">Diğer (Manuel Gir)</option>
                      </select>
                      {formData[col] === 'Diğer' && (
                        <input
                          className="form-input"
                          type="number"
                          style={{ marginTop: '0.5rem' }}
                          placeholder="Kalınlık giriniz (mm)"
                          value={formData[`${col}Custom`] || ''}
                          onChange={(e) => setFormData({ ...formData, [`${col}Custom`]: e.target.value })}
                        />
                      )}
                    </div>
                  );
                }

                if (col === 'weight') {
                  return (
                    <div className="form-group" key={col}>
                      <label className="form-label">{columnLabels[col]}</label>
                      <select
                        className="form-input"
                        value={formData[col] || ''}
                        onChange={(e) => setFormData({ ...formData, [col]: e.target.value === 'Diğer' ? 'Diğer' : parseFloat(e.target.value) || '' })}
                      >
                        <option value="">Seçiniz...</option>
                        {popularWeights.map((w) => (
                          <option key={w} value={w}>{w}</option>
                        ))}
                        <option value="Diğer">Diğer (Manuel Gir)</option>
                      </select>
                      {formData[col] === 'Diğer' && (
                        <input
                          className="form-input"
                          type="number"
                          style={{ marginTop: '0.5rem' }}
                          placeholder="Ağırlık/Ölçü giriniz"
                          value={formData[`${col}Custom`] || ''}
                          onChange={(e) => setFormData({ ...formData, [`${col}Custom`]: e.target.value })}
                        />
                      )}
                    </div>
                  );
                }

                if (col === 'color') {
                  return (
                    <div className="form-group" key={col}>
                      <label className="form-label">{columnLabels[col]}</label>
                      <select
                        className="form-input"
                        value={formData[col] || ''}
                        onChange={(e) => setFormData({ ...formData, [col]: e.target.value })}
                      >
                        <option value="">Seçiniz...</option>
                        {popularColors.map((c) => (
                          <option key={c} value={c}>{c}</option>
                        ))}
                      </select>
                      {formData[col] === 'Diğer' && (
                        <input
                          className="form-input"
                          type="text"
                          style={{ marginTop: '0.5rem' }}
                          placeholder="Renk giriniz"
                          value={formData[`${col}Custom`] || ''}
                          onChange={(e) => setFormData({ ...formData, [`${col}Custom`]: e.target.value })}
                        />
                      )}
                    </div>
                  );
                }

                if (col === 'materialType') {
                  return (
                    <div className="form-group" key={col}>
                      <label className="form-label">{columnLabels[col]}</label>
                      <select
                        className="form-input"
                        value={formData[col] || ''}
                        onChange={(e) => setFormData({ ...formData, [col]: e.target.value })}
                      >
                        <option value="">Seçiniz...</option>
                        <option value="MDF">MDF</option>
                        <option value="Sunta">Sunta</option>
                        <option value="Kontrplak">Kontrplak</option>
                        <option value="Diğer">Diğer</option>
                      </select>
                      {formData[col] === 'Diğer' && (
                        <input
                          className="form-input"
                          type="text"
                          style={{ marginTop: '0.5rem' }}
                          placeholder="Tür adını giriniz"
                          value={formData[`${col}Custom`] || ''}
                          onChange={(e) => setFormData({ ...formData, [`${col}Custom`]: e.target.value })}
                        />
                      )}
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

      {/* Reduce Modal */}
      {showReduceModal && (
        <div className="modal-overlay" onClick={() => setShowReduceModal(false)}>
          <div className="modal" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h3>Stok Azalt</h3>
              <button className="modal-close" onClick={() => setShowReduceModal(false)}>
                <FiX />
              </button>
            </div>
            <div className="modal-body">
              <div className="form-group">
                <label className="form-label">Eksiltilecek Miktar</label>
                <input
                  className="form-input"
                  type="number"
                  placeholder="Miktar giriniz"
                  value={reduceCount}
                  onChange={(e) => setReduceCount(e.target.value)}
                  min="1"
                />
              </div>
            </div>
            <div className="modal-footer">
              <button className="btn-cancel" onClick={() => setShowReduceModal(false)}>İptal</button>
              <button className="btn-save" onClick={handleReduce}>Kaydet</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
