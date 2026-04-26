import { useState, useEffect } from 'react';
import { useAuth } from '../../context/AuthContext';
import { authService } from '../../services/api';
import { FiUser, FiBriefcase, FiSave } from 'react-icons/fi';
import './Profile.css';

export default function Profile() {
  const { user } = useAuth();
  
  // States
  const [loading1, setLoading1] = useState(false);
  const [loading2, setLoading2] = useState(false);
  const [message1, setMessage1] = useState({ text: '', type: '' });
  const [message2, setMessage2] = useState({ text: '', type: '' });

  // Forms
  const [personalData, setPersonalData] = useState({
    fullName: '',
    city: '',
  });

  const [jobData, setJobData] = useState({
    companyName: '',
    taxNumber: '',
    taxOffice: '',
    address: '',
  });

  useEffect(() => {
    // Profil ilk yüklendiğinde mevcut kullanıcı bilgilerini dolduruyoruz
    if (user) {
      setPersonalData({
        fullName: user.fullName || '',
        city: user.city || '',
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
              <label className="form-label">Şehir</label>
              <input
                type="text"
                className="form-input"
                placeholder="Örn: İstanbul"
                value={personalData.city}
                onChange={(e) => setPersonalData({ ...personalData, city: e.target.value })}
              />
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
              <label className="form-label">Firma Adı</label>
              <input
                type="text"
                className="form-input"
                placeholder="Firma ünvanı"
                value={jobData.companyName}
                onChange={(e) => setJobData({ ...jobData, companyName: e.target.value })}
                required
              />
            </div>

            <div className="form-group">
              <label className="form-label">Vergi Dairesi</label>
              <input
                type="text"
                className="form-input"
                placeholder="Vergi Dairesi"
                value={jobData.taxOffice}
                onChange={(e) => setJobData({ ...jobData, taxOffice: e.target.value })}
              />
            </div>

            <div className="form-group">
              <label className="form-label">Vergi Numarası</label>
              <input
                type="text"
                className="form-input"
                placeholder="Vergi No / TC"
                value={jobData.taxNumber}
                onChange={(e) => setJobData({ ...jobData, taxNumber: e.target.value })}
              />
            </div>

            <div className="form-group">
              <label className="form-label">Adres</label>
              <textarea
                className="form-input"
                placeholder="Firma açık adresi"
                rows="3"
                value={jobData.address}
                onChange={(e) => setJobData({ ...jobData, address: e.target.value })}
                style={{ resize: 'vertical' }}
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
