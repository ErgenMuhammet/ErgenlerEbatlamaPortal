import { useState, useEffect } from 'react';
import { useSearchParams, useNavigate } from 'react-router-dom';
import { authService } from '../../services/api';
import '../Login/Login.css'; // Re-use the login styles
import { FiEye, FiEyeOff } from 'react-icons/fi';

export default function ResetPassword() {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const [userId, setUserId] = useState('');
  const [token, setToken] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [showPassword, setShowPassword] = useState(false);

  useEffect(() => {
    const id = searchParams.get('userId');
    const tk = searchParams.get('token');
    if (id && tk) {
      setUserId(id);
      setToken(tk);
    } else {
      setError('Geçersiz veya eksik şifre sıfırlama bağlantısı.');
    }
  }, [searchParams]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (newPassword !== confirmPassword) {
      setError('Şifreler birbiriyle uyuşmuyor.');
      return;
    }
    
    setError('');
    setSuccess('');
    setLoading(true);

    try {
      const res = await authService.resetPassword(userId, token, { 
        Password: newPassword, 
        PasswordConfirm: confirmPassword 
      });
      
      if (res.data.isSuccess || res.data.success) {
        setSuccess('Şifreniz başarıyla sıfırlandı. Giriş yapabilirsiniz.');
        setTimeout(() => {
          navigate('/login');
        }, 3000);
      } else {
        setError(res.data.message || 'Şifre sıfırlama başarısız.');
      }
    } catch (err) {
      setError('Bir hata oluştu. Linkin süresi dolmuş veya geçersiz olabilir.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-page">
      <div className="login-card glass-card">
        <div className="login-header">
          <div className="login-logo">EP</div>
          <h1>Yeni Şifre Belirle</h1>
          <p>Lütfen yeni şifrenizi giriniz</p>
        </div>

        {error && <div className="login-error">{error}</div>}
        {success && <div className="login-success" style={{ color: 'var(--success-color)', background: 'rgba(34,197,94,0.1)', padding: '0.75rem', borderRadius: '0.5rem', marginBottom: '1rem', fontSize: '0.875rem' }}>{success}</div>}

        <form className="login-form" onSubmit={handleSubmit}>
          <div className="form-group">
            <label className="form-label" htmlFor="new-password">Yeni Şifre</label>
            <div style={{ position: 'relative' }}>
              <input
                id="new-password"
                className="form-input"
                type={showPassword ? "text" : "password"}
                placeholder="••••••••"
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
                required
                disabled={loading || success || !userId}
                style={{ paddingRight: '2.5rem' }}
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
                  color: 'var(--text-muted)',
                  cursor: 'pointer',
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  padding: '0.2rem'
                }}
              >
                {showPassword ? <FiEyeOff size={18} /> : <FiEye size={18} />}
              </button>
            </div>
          </div>
          <div className="form-group">
            <label className="form-label" htmlFor="confirm-password">Şifre (Tekrar)</label>
            <input
              id="confirm-password"
              className="form-input"
              type={showPassword ? "text" : "password"}
              placeholder="••••••••"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
              disabled={loading || success || !userId}
            />
          </div>

          <button className="login-btn" type="submit" disabled={loading || success || !userId}>
            {loading && <span className="spinner" />}
            {loading ? 'İşleniyor...' : 'Şifreyi Kaydet'}
          </button>
          
          <button 
            type="button" 
            className="login-btn btn-secondary" 
            style={{ marginTop: '1rem' }}
            onClick={() => navigate('/login')}
          >
            Giriş Ekranına Dön
          </button>
        </form>
      </div>
    </div>
  );
}
