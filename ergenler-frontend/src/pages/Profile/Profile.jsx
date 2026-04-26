import { useState, useEffect } from 'react';
import { useAuth } from '../../context/AuthContext';
import { authService } from '../../services/api';
import { FiUser, FiBriefcase, FiSave, FiEye, FiEyeOff } from 'react-icons/fi';
import './Profile.css';

const TURKEY_CITIES = [
  "Adana", "Adıyaman", "Afyonkarahisar", "Ağrı", "Amasya", "Ankara", "Antalya", "Artvin", "Aydın", "Balıkesir",
  "Bilecik", "Bingöl", "Bitlis", "Bolu", "Burdur", "Bursa", "Çanakkale", "Çankırı", "Çorum", "Denizli",
  "Diyarbakır", "Edirne", "Elazığ", "Erzincan", "Erzurum", "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane", "Hakkari",
  "Hatay", "Isparta", "Mersin", "İstanbul", "İzmir", "Kars", "Kastamonu", "Kayseri", "Kırklareli", "Kırşehir",
  "Kocaeli", "Konya", "Kütahya", "Malatya", "Manisa", "Kahramanmaraş", "Mardin", "Muğla", "Muş", "Nevşehir",
  "Niğde", "Ordu", "Rize", "Sakarya", "Samsun", "Siirt", "Sinop", "Sivas", "Tekirdağ", "Tokat",
  "Trabzon", "Tunceli", "Şanlıurfa", "Uşak", "Van", "Yozgat", "Zonguldak", "Aksaray", "Bayburt", "Karaman",
  "Kırıkkale", "Batman", "Şırnak", "Bartın", "Ardahan", "Iğdır", "Yalova", "Karabük", "Kilis", "Osmaniye", "Düzce"
];

export default function Profile() {
  const { user } = useAuth();
  
  // States
  const [loading1, setLoading1] = useState(false);
  const [loading2, setLoading2] = useState(false);
  const [message1, setMessage1] = useState({ text: '', type: '' });
  const [message2, setMessage2] = useState({ text: '', type: '' });
  const [showPassword, setShowPassword] = useState(false);

  // Forms
  const [personalData, setPersonalData] = useState({
    fullName: '',
    city: '',
    email: '',
    password: '',
    birthDate: '',
  });

  const [jobData, setJobData] = useState({
    workShopName: '',
    adressDescription: '',
    experience: 0,
    phoneNumber: '',
  });

  useEffect(() => {
    // Profil ilk yüklendiğinde mevcut kullanıcı bilgilerini dolduruyoruz
    if (user) {
      setPersonalData({
        fullName: user.fullName || '',
        city: user.city || '',
        email: user.email || '',
        password: '',
        birthDate: user.birthDate ? new Date(user.birthDate).toISOString().split('T')[0] : '',
      });
      // Eğer user modelinde iş bilgileri geliyorsa buraya eklenebilir
    }
  }, [user]);

  const handlePersonalUpdate = async (e) => {
    e.preventDefault();
    setLoading1(true);
    setMessage1({ text: '', type: '' });
    
    try {
      const res = await authService.updateMyProperty(personalData);
      if (res.data?.isSuccess || res.data?.isSucces || res.status === 200) {
        setMessage1({ text: 'Kişisel bilgiler başarıyla güncellendi.', type: 'success' });
      } else {
        setMessage1({ text: 'Güncelleme başarısız oldu.', type: 'error' });
      }
    } catch (err) {
      setMessage1({ text: 'Bir hata oluştu.', type: 'error' });
    } finally {
      setLoading1(false);
    }
  };

  const handleJobUpdate = async (e) => {
    e.preventDefault();
    setLoading2(true);
    setMessage2({ text: '', type: '' });
    
    try {
      const res = await authService.updateMyJobsProperty(jobData);
      if (res.data?.isSuccess || res.data?.isSucces || res.status === 200) {
        setMessage2({ text: 'İş bilgileri başarıyla güncellendi.', type: 'success' });
      } else {
        setMessage2({ text: 'Güncelleme başarısız oldu.', type: 'error' });
      }
    } catch (err) {
      setMessage2({ text: 'Bir hata oluştu.', type: 'error' });
    } finally {
      setLoading2(false);
    }
  };

  return (
    <div className="profile-page">
      <div className="profile-header">
        <h1>Profil Ayarları</h1>
        <p>Kişisel ve iş bilgilerinizi bu alandan güncelleyebilirsiniz.</p>
      </div>

      <div className="profile-grid">
        {/* Personal Details Form */}
        <div className="profile-card">
          <div className="profile-card-title">
            <FiUser /> Kişisel Bilgiler
          </div>
          
          {message1.text && (
            <div className={`alert-message ${message1.type === 'success' ? 'alert-success' : 'alert-error'}`}>
              {message1.text}
            </div>
          )}

          <form className="profile-form" onSubmit={handlePersonalUpdate}>
            <div className="form-group">
              <label className="form-label">Ad Soyad</label>
              <input
                type="text"
                className="form-input"
                placeholder="Ad Soyad"
                value={personalData.fullName}
                onChange={(e) => setPersonalData({ ...personalData, fullName: e.target.value })}
                required
              />
            </div>
            
            <div className="form-group">
              <label className="form-label">E-Posta Adresi</label>
              <input
                type="email"
                className="form-input"
                placeholder="Örn: ornek@mail.com"
                value={personalData.email}
                onChange={(e) => setPersonalData({ ...personalData, email: e.target.value })}
              />
            </div>

            <div className="form-group">
              <label className="form-label">Yeni Şifre (Değiştirmek istemiyorsanız boş bırakın)</label>
              <div style={{ position: 'relative' }}>
                <input
                  type={showPassword ? 'text' : 'password'}
                  className="form-input"
                  placeholder="Yeni şifreniz"
                  value={personalData.password}
                  onChange={(e) => setPersonalData({ ...personalData, password: e.target.value })}
                />
                <button
                  type="button"
                  onClick={() => setShowPassword(!showPassword)}
                  style={{
                    position: 'absolute',
                    right: '0.75rem',
                    top: '50%',
                    transform: 'translateY(-50%)',
                    background: 'none',
                    border: 'none',
                    color: 'var(--text-secondary)',
                    cursor: 'pointer',
                    display: 'flex',
                    alignItems: 'center',
                    padding: '0.25rem'
                  }}
                  title={showPassword ? "Şifreyi Gizle" : "Şifreyi Göster"}
                >
                  {showPassword ? <FiEyeOff size={18} /> : <FiEye size={18} />}
                </button>
              </div>
            </div>

            <div className="form-group">
              <label className="form-label">Doğum Tarihi</label>
              <input
                type="date"
                className="form-input"
                value={personalData.birthDate}
                onChange={(e) => setPersonalData({ ...personalData, birthDate: e.target.value })}
              />
            </div>
            
            <div className="form-group">
              <label className="form-label">Şehir</label>
              <input
                type="text"
                className="form-input"
                placeholder="Örn: İstanbul"
                list="turkey-cities"
                value={personalData.city}
                onChange={(e) => setPersonalData({ ...personalData, city: e.target.value })}
              />
              <datalist id="turkey-cities">
                {TURKEY_CITIES.map((city) => (
                  <option key={city} value={city} />
                ))}
              </datalist>
            </div>

            <button type="submit" className="profile-btn" disabled={loading1}>
              {loading1 ? <span className="spinner" /> : <FiSave />}
              {loading1 ? 'Kaydediliyor...' : 'Bilgileri Kaydet'}
            </button>
          </form>
        </div>

        {/* Job Details Form */}
        <div className="profile-card">
          <div className="profile-card-title">
            <FiBriefcase /> İş / Firma Bilgileri
          </div>

          {message2.text && (
            <div className={`alert-message ${message2.type === 'success' ? 'alert-success' : 'alert-error'}`}>
              {message2.text}
            </div>
          )}

          <form className="profile-form" onSubmit={handleJobUpdate}>
            <div className="form-group">
              <label className="form-label">İşletme/Firma Adı</label>
              <input
                type="text"
                className="form-input"
                placeholder="İşletme/Firma ünvanı"
                value={jobData.workShopName}
                onChange={(e) => setJobData({ ...jobData, workShopName: e.target.value })}
                required
              />
            </div>

            <div className="form-group">
              <label className="form-label">Adres Açıklaması</label>
              <textarea
                className="form-input"
                placeholder="Firma açık adresi"
                rows="3"
                value={jobData.adressDescription}
                onChange={(e) => setJobData({ ...jobData, adressDescription: e.target.value })}
                style={{ resize: 'vertical' }}
              />
            </div>

            <div className="form-group">
              <label className="form-label">Tecrübe (Yıl)</label>
              <input
                type="number"
                className="form-input"
                placeholder="Örn: 5"
                value={jobData.experience}
                onChange={(e) => setJobData({ ...jobData, experience: parseInt(e.target.value) || 0 })}
              />
            </div>

            <div className="form-group">
              <label className="form-label">Telefon Numarası</label>
              <input
                type="text"
                className="form-input"
                placeholder="Örn: 05xx xxx xx xx"
                value={jobData.phoneNumber}
                onChange={(e) => setJobData({ ...jobData, phoneNumber: e.target.value })}
              />
            </div>

            <button type="submit" className="profile-btn" disabled={loading2}>
              {loading2 ? <span className="spinner" /> : <FiSave />}
              {loading2 ? 'Kaydediliyor...' : 'İş Bilgilerini Kaydet'}
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}
