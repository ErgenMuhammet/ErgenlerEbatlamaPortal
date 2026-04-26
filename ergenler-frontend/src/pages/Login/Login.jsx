import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import './Login.css';

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

export default function Login() {
  const [isLogin, setIsLogin] = useState(true);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const { login, register } = useAuth();
  const navigate = useNavigate();

  // Login form state
  const [loginData, setLoginData] = useState({ email: '', password: '' });

  // Register form state
  const [registerData, setRegisterData] = useState({
    fullName: '',
    birthDate: '',
    userCategory: 1,
    email: '',
    password: '',
    passwordConfirm: '',
    phoneNumber: '',
    city: '',
  });

  const handleLogin = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      const result = await login(loginData);
      if (result.success) {
        navigate('/dashboard');
      } else {
        setError(result.message);
      }
    } catch (err) {
      if (!err.response) {
        setError('Sunucuya ulaşılamıyor veya ağ hatası.');
      } else {
        setError('Kullanıcı adı veya şifre hatalı');
      }
    } finally {
      setLoading(false);
    }
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      const result = await register(registerData);
      if (result.isSucces) {
        setIsLogin(true);
        setError('');
      } else {
        setError(result.message || 'Kayıt başarısız');
      }
    } catch (err) {
      setError('Kayıt sırasında bir hata oluştu');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-page">
      <div className="login-card glass-card">
        <div className="login-header">
          <div className="login-logo">EP</div>
          <h1>Ergenler Portal</h1>
          <p>Ebatlama Yönetim Sistemi</p>
        </div>

        {/* Tab Switcher */}
        <div className="auth-tabs">
          <button
            className={`auth-tab ${isLogin ? 'active' : ''}`}
            onClick={() => { setIsLogin(true); setError(''); }}
          >
            Giriş Yap
          </button>
          <button
            className={`auth-tab ${!isLogin ? 'active' : ''}`}
            onClick={() => { setIsLogin(false); setError(''); }}
          >
            Kayıt Ol
          </button>
        </div>

        {error && <div className="login-error">{error}</div>}

        {isLogin ? (
          <form className="login-form" onSubmit={handleLogin}>
            <div className="form-group">
              <label className="form-label" htmlFor="login-email">E-Posta</label>
              <input
                id="login-email"
                className="form-input"
                type="email"
                placeholder="ornek@mail.com"
                value={loginData.email}
                onChange={(e) => setLoginData({ ...loginData, email: e.target.value })}
                required
              />
            </div>
            <div className="form-group">
              <label className="form-label" htmlFor="login-password">Şifre</label>
              <input
                id="login-password"
                className="form-input"
                type="password"
                placeholder="••••••••"
                value={loginData.password}
                onChange={(e) => setLoginData({ ...loginData, password: e.target.value })}
                required
              />
            </div>
            <button className="login-btn" type="submit" disabled={loading}>
              {loading && <span className="spinner" />}
              {loading ? 'Giriş yapılıyor...' : 'Giriş Yap'}
            </button>
          </form>
        ) : (
          <form className="login-form" onSubmit={handleRegister}>
            <div className="form-group">
              <label className="form-label" htmlFor="reg-fullname">Ad Soyad</label>
              <input
                id="reg-fullname"
                className="form-input"
                type="text"
                placeholder="Ad Soyad"
                value={registerData.fullName}
                onChange={(e) => setRegisterData({ ...registerData, fullName: e.target.value })}
                required
              />
            </div>
            <div className="form-group">
              <label className="form-label" htmlFor="reg-email">E-Posta</label>
              <input
                id="reg-email"
                className="form-input"
                type="email"
                placeholder="ornek@mail.com"
                value={registerData.email}
                onChange={(e) => setRegisterData({ ...registerData, email: e.target.value })}
                required
              />
            </div>
            <div className="form-group">
              <label className="form-label" htmlFor="reg-phone">Telefon</label>
              <input
                id="reg-phone"
                className="form-input"
                type="tel"
                placeholder="05xxxxxxxxx"
                value={registerData.phoneNumber}
                onChange={(e) => setRegisterData({ ...registerData, phoneNumber: e.target.value })}
              />
            </div>
            <div className="form-group">
              <label className="form-label" htmlFor="reg-birthdate">Doğum Tarihi</label>
              <input
                id="reg-birthdate"
                className="form-input"
                type="date"
                value={registerData.birthDate}
                onChange={(e) => setRegisterData({ ...registerData, birthDate: e.target.value })}
              />
            </div>
            <div className="form-group">
              <label className="form-label" htmlFor="reg-category">Kategori</label>
              <select
                id="reg-category"
                className="form-input"
                value={registerData.userCategory}
                onChange={(e) => setRegisterData({ ...registerData, userCategory: Number(e.target.value) })}
              >
                <option value={1}>Marangoz</option>
                <option value={2}>Montajcı</option>
                <option value={4}>Ebatlamacı</option>
              </select>
            </div>
            <div className="form-group">
              <label className="form-label" htmlFor="reg-city">Şehir</label>
              <input
                id="reg-city"
                className="form-input"
                type="text"
                placeholder="Örn: İstanbul"
                list="turkey-cities-reg"
                value={registerData.city}
                onChange={(e) => setRegisterData({ ...registerData, city: e.target.value })}
              />
              <datalist id="turkey-cities-reg">
                {TURKEY_CITIES.map((city) => (
                  <option key={city} value={city} />
                ))}
              </datalist>
            </div>
            <div className="form-group">
              <label className="form-label" htmlFor="reg-password">Şifre</label>
              <input
                id="reg-password"
                className="form-input"
                type="password"
                placeholder="••••••••"
                value={registerData.password}
                onChange={(e) => setRegisterData({ ...registerData, password: e.target.value })}
                required
              />
            </div>
            <div className="form-group">
              <label className="form-label" htmlFor="reg-password-confirm">Şifre Tekrar</label>
              <input
                id="reg-password-confirm"
                className="form-input"
                type="password"
                placeholder="••••••••"
                value={registerData.passwordConfirm}
                onChange={(e) => setRegisterData({ ...registerData, passwordConfirm: e.target.value })}
                required
              />
            </div>
            <button className="login-btn" type="submit" disabled={loading}>
              {loading && <span className="spinner" />}
              {loading ? 'Kayıt yapılıyor...' : 'Kayıt Ol'}
            </button>
          </form>
        )}
      </div>
    </div>
  );
}
