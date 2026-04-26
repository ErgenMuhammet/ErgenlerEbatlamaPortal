import { useState, useEffect } from 'react';
import { advertisementService } from '../../services/api';
import { FiPlus, FiMapPin, FiStar, FiX } from 'react-icons/fi';
import './Advertisements.css';

const categoryLabels = {
  1: 'Marangoz',
  2: 'Montajcı',
  4: 'Ebatlamacı',
  3: 'Marangoz & Montajcı',
  5: 'Marangoz & Ebatlamacı',
  6: 'Montajcı & Ebatlamacı',
  7: 'Tümü',
};

export default function Advertisements() {
  const [ads, setAds] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState({
    title: '',
    advertisementAddress: '',
    imgUrl: '',
    targetCategory: 3,
  });

  useEffect(() => {
    loadAds();
  }, []);

  const loadAds = async () => {
    setLoading(true);
    try {
      const res = await advertisementService.getAll();
      setAds(res.data?.data || []);
    } catch {
      setAds([]);
    } finally {
      setLoading(false);
    }
  };

  const handleAdd = async () => {
    try {
      await advertisementService.add(formData);
      setShowModal(false);
      setFormData({ title: '', advertisementAddress: '', imgUrl: '', targetCategory: 3 });
      loadAds();
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <div className="ads-page">
      <div className="ads-toolbar">
        <div />
        <button className="add-btn" onClick={() => setShowModal(true)}>
          <FiPlus /> Yeni İlan
        </button>
      </div>

      {loading ? (
        <div className="loading-container">
          <div className="loading-spinner" />
        </div>
      ) : ads.length === 0 ? (
        <div className="empty-state">
          <div className="empty-state-icon"><FiStar /></div>
          <p>Henüz ilan bulunmuyor</p>
        </div>
      ) : (
        <div className="ads-grid">
          {ads.map((ad) => (
            <div key={ad.advertisementId} className="ad-card slide-up">
              <div className="ad-card-image">
                {ad.imgUrl ? (
                  <img src={ad.imgUrl} alt={ad.title} />
                ) : (
                  <span className="ad-card-placeholder"><FiStar /></span>
                )}
              </div>
              <div className="ad-card-content">
                <div className="ad-card-title">{ad.title}</div>
                <div className="ad-card-address">
                  <FiMapPin /> {ad.advertisementAddress || 'Adres belirtilmemiş'}
                </div>
                <div className="ad-card-meta">
                  <span className="ad-card-date">
                    {ad.advertisementDate
                      ? new Date(ad.advertisementDate).toLocaleDateString('tr-TR')
                      : '-'}
                  </span>
                  <span className="ad-card-category">
                    {categoryLabels[ad.targetCategory] || 'Genel'}
                  </span>
                </div>
              </div>
              <div className="ad-card-actions">
                <button className="ad-action-btn offer">Teklif Ver</button>
              </div>
            </div>
          ))}
        </div>
      )}

      {/* Add Modal */}
      {showModal && (
        <div className="modal-overlay" onClick={() => setShowModal(false)}>
          <div className="modal" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h3>Yeni İlan Ekle</h3>
              <button className="modal-close" onClick={() => setShowModal(false)}>
                <FiX />
              </button>
            </div>
            <div className="modal-body">
              <div className="form-group">
                <label className="form-label">Başlık</label>
                <input
                  className="form-input"
                  type="text"
                  placeholder="İlan başlığı"
                  value={formData.title}
                  onChange={(e) => setFormData({ ...formData, title: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label className="form-label">Adres</label>
                <input
                  className="form-input"
                  type="text"
                  placeholder="Adres"
                  value={formData.advertisementAddress}
                  onChange={(e) => setFormData({ ...formData, advertisementAddress: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label className="form-label">Görsel URL</label>
                <input
                  className="form-input"
                  type="text"
                  placeholder="https://..."
                  value={formData.imgUrl}
                  onChange={(e) => setFormData({ ...formData, imgUrl: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label className="form-label">Hedef Kategori</label>
                <select
                  className="form-input"
                  value={formData.targetCategory}
                  onChange={(e) => setFormData({ ...formData, targetCategory: Number(e.target.value) })}
                >
                  {Object.entries(categoryLabels).map(([val, label]) => (
                    <option key={val} value={val}>{label}</option>
                  ))}
                </select>
              </div>
            </div>
            <div className="modal-footer">
              <button className="btn-cancel" onClick={() => setShowModal(false)}>İptal</button>
              <button className="btn-save" onClick={handleAdd}>Yayınla</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
