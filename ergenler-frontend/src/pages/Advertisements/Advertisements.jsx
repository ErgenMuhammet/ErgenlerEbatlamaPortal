import { useState, useEffect } from 'react';
import { advertisementService } from '../../services/api';
import { FiPlus, FiMapPin, FiStar, FiX, FiEdit2, FiTrash2, FiUser } from 'react-icons/fi';
import { useAuth } from '../../context/AuthContext';
import { MapContainer, TileLayer, Marker, useMapEvents } from 'react-leaflet';
import 'leaflet/dist/leaflet.css';
import L from 'leaflet';
import './Advertisements.css';

// Fix leaflet icon issue
delete L.Icon.Default.prototype._getIconUrl;
L.Icon.Default.mergeOptions({
  iconRetinaUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon-2x.png',
  iconUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon.png',
  shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
});

function LocationPicker({ position, setPosition }) {
  useMapEvents({
    click(e) {
      setPosition({ lat: e.latlng.lat, lng: e.latlng.lng });
    },
  });
  return position ? <Marker position={position} /> : null;
}

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
  const { user } = useAuth();
  const [ads, setAds] = useState([]);
  
  const formatName = (str) => {
    if (!str) return 'Bilinmeyen Kullanıcı';
    return str.toLowerCase().split(' ').map(w => w.charAt(0).toUpperCase() + w.slice(1)).join(' ');
  };

  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState({
    title: '',
    advertisementAddress: '',
    imgUrl: '',
    targetCategory: 3,
    latitude: null,
    longitude: null,
  });
  const [mapPosition, setMapPosition] = useState(null);
  const [viewMode, setViewMode] = useState('all'); // 'all' or 'mine'
  const [editId, setEditId] = useState(null);

  const loadAds = async (currentMode = viewMode) => {
    setLoading(true);
    try {
      const res = currentMode === 'mine' 
        ? await advertisementService.getMine() 
        : await advertisementService.getAll();
      
      console.log(`${currentMode} API Yanıtı:`, res);
      
      // Backend'den gelen tüm olası isimlendirmeleri kontrol et (advs, Advs vb.)
      const adsList = res.data?.advs || res.data?.Advs || [];
      setAds(adsList);
    } catch (err) {
      console.error("İlanlar yüklenirken hata:", err);
      setAds([]);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadAds(viewMode);
  }, [viewMode]);

  const handleSave = async () => {
    try {
      const payload = {
        ...formData,
        latitude: mapPosition ? mapPosition.lat : null,
        longitude: mapPosition ? mapPosition.lng : null
      };

      if (editId) {
        await advertisementService.update(editId, payload);
      } else {
        await advertisementService.add(payload);
      }

      setShowModal(false);
      resetForm();
      loadAds();
    } catch (err) {
      console.error(err);
    }
  };

  const handleEditClick = (ad) => {
    setEditId(ad.id);
    setFormData({
      title: ad.title || '',
      advertisementAddress: ad.advertisementAddress || '',
      imgUrl: ad.imgUrl || '',
      targetCategory: ad.targetCategory || 3,
      latitude: ad.latitude || null,
      longitude: ad.longitude || null,
    });
    setMapPosition(ad.latitude && ad.longitude ? { lat: ad.latitude, lng: ad.longitude } : null);
    setShowModal(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Bu ilanı silmek istediğinize emin misiniz?')) {
      try {
        await advertisementService.delete(id);
        loadAds();
      } catch (err) {
        console.error(err);
      }
    }
  };

  const resetForm = () => {
    setEditId(null);
    setFormData({ title: '', advertisementAddress: '', imgUrl: '', targetCategory: 3, latitude: null, longitude: null });
    setMapPosition(null);
  };

  const handleCloseModal = () => {
    setShowModal(false);
    resetForm();
  };

  return (
    <div className="ads-page">
      <div className="ads-toolbar">
        <div className="ads-tabs">
          <button
            className={`tab-btn ${viewMode === 'all' ? 'active' : ''}`}
            onClick={() => setViewMode('all')}
          >
            Tüm İlanlar
          </button>
          <button
            className={`tab-btn ${viewMode === 'mine' ? 'active' : ''}`}
            onClick={() => setViewMode('mine')}
          >
            Benim İlanlarım
          </button>
        </div>
        <button className="add-btn" onClick={() => { resetForm(); setShowModal(true); }}>
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
            <div key={ad.id} className="ad-card slide-up">
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
                <div className="ad-card-owner">
                  <FiUser /> {formatName(ad.ownerName)}
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
                {ad.ownerId === user?.id ? (
                  <>
                    <button className="ad-action-btn edit" onClick={() => handleEditClick(ad)}>
                      <FiEdit2 /> Düzenle
                    </button>
                    <button className="ad-action-btn delete" onClick={() => handleDelete(ad.id)}>
                      <FiTrash2 /> Sil
                    </button>
                  </>
                ) : (
                  <button className="ad-action-btn offer" onClick={() => handleOffer(ad.id)}>Teklif Ver</button>
                )}
              </div>
            </div>
          ))}
        </div>
      )}

      {/* Add Modal */}
      {showModal && (
        <div className="modal-overlay" onClick={handleCloseModal}>
          <div className="modal" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h3>{editId ? 'İlanı Düzenle' : 'Yeni İlan Ekle'}</h3>
              <button className="modal-close" onClick={handleCloseModal}>
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
                <label className="form-label">Haritadan Konum Seç (İsteğe Bağlı)</label>
                <div style={{ height: '200px', width: '100%', borderRadius: '8px', overflow: 'hidden', border: '1px solid var(--border-color)' }}>
                  <MapContainer
                    center={[39.92077, 32.85411]}
                    zoom={5}
                    style={{ height: '100%', width: '100%' }}
                  >
                    <TileLayer
                      url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                      attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                    />
                    <LocationPicker position={mapPosition} setPosition={setMapPosition} />
                  </MapContainer>
                </div>
                {mapPosition && (
                  <div style={{ fontSize: '0.8rem', marginTop: '0.5rem', color: 'var(--text-muted)' }}>
                    Seçilen Koordinat: {mapPosition.lat.toFixed(4)}, {mapPosition.lng.toFixed(4)}
                    <button
                      type="button"
                      onClick={() => setMapPosition(null)}
                      style={{ background: 'none', border: 'none', color: 'var(--danger-color)', marginLeft: '10px', cursor: 'pointer' }}
                    >
                      Konumu Temizle
                    </button>
                  </div>
                )}
              </div>
            </div>
            <div className="modal-footer">
              <button className="btn-cancel" onClick={handleCloseModal}>İptal</button>
              <button className="btn-save" onClick={handleSave}>{editId ? 'Güncelle' : 'Yayınla'}</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
