import { useState, useEffect } from 'react';
import { NavLink, Outlet, useLocation } from 'react-router-dom';
import { authService } from '../../services/api';
import { useAuth } from '../../context/AuthContext';
import {
  FiHome, FiPackage, FiShoppingCart, FiDollarSign,
  FiStar, FiLogOut, FiMenu, FiX, FiUser, FiLayers,
  FiMessageSquare, FiBell
} from 'react-icons/fi';
import './Layout.css';

const navItems = [
  {
    section: 'Ana Menü',
    items: [
      { to: '/dashboard', label: 'Genel Bakış', icon: <FiHome /> },
    ],
  },
  {
    section: 'Stok Yönetimi',
    items: [
      { to: '/stock', label: 'Stok Durumu', icon: <FiPackage /> },
    ],
  },
  {
    section: 'İş Yönetimi',
    items: [
      { to: '/orders', label: 'Siparişler', icon: <FiShoppingCart /> },
      { to: '/measurements', label: 'Ebatlama & Ölçüler', icon: <FiLayers /> },
      { to: '/advertisements', label: 'İlanlar', icon: <FiStar /> },
      { to: '/chat', label: 'Mesajlar', icon: <FiMessageSquare /> },
    ],
  },
  {
    section: 'Muhasebe',
    items: [
      { to: '/accounting', label: 'Gelir & Gider', icon: <FiDollarSign /> },
    ],
  },
  {
    section: 'Ayarlar',
    items: [
      { to: '/profile', label: 'Profilim', icon: <FiUser /> },
    ],
  },
];

const pageTitles = {
  '/dashboard': 'Genel Bakış',
  '/stock': 'Stok Yönetimi',
  '/orders': 'Sipariş Yönetimi',
  '/measurements': 'Ebatlama & Ölçü Detayları',
  '/advertisements': 'İlan Yönetimi',
  '/accounting': 'Muhasebe',
  '/profile': 'Profil Ayarları',
  '/chat': 'Mesajlaşma',
};

export default function Layout() {
  const { user, logout } = useAuth();
  const location = useLocation();
  const [sidebarOpen, setSidebarOpen] = useState(false);

  const currentTitle = pageTitles[location.pathname] || 'Portal';
  
  const formatName = (str) => {
    if (!str) return 'Kullanıcı';
    return str.toLowerCase().split(' ').map(w => w.charAt(0).toUpperCase() + w.slice(1)).join(' ');
  };

  const userName = formatName(user?.name || user?.fullName);
  const initials = userName.split(' ').map((n) => n[0]).join('').toUpperCase();

  const [notifications, setNotifications] = useState([]);
  const [showNotifications, setShowNotifications] = useState(false);

  useEffect(() => {
    const fetchNotifications = async () => {
      try {
        const res = await authService.getMyNotifications();
        if (res.data?.isSuccess) {
          setNotifications(res.data.notifications || []);
        }
      } catch (err) {
        console.error("Bildirimler alınamadı:", err);
      }
    };
    if (user) fetchNotifications();
  }, [user]);

  return (
    <div className="layout">
      {/* Mobile overlay */}
      <div
        className={`sidebar-overlay ${sidebarOpen ? 'visible' : ''}`}
        onClick={() => setSidebarOpen(false)}
      />

      {/* Sidebar */}
      <aside className={`sidebar ${sidebarOpen ? 'open' : ''}`}>
        <div className="sidebar-header">
          <div className="sidebar-logo">
            <div className="sidebar-logo-icon">EP</div>
            <div className="sidebar-logo-text">
              <h1>Ergenler Portal</h1>
              <span style={{ fontSize: '0.85rem', color: 'var(--primary-color)' }}>
                {userName}
              </span>
            </div>
          </div>
        </div>

        <nav className="sidebar-nav">
          {navItems.map((section) => (
            <div key={section.section} className="nav-section">
              <div className="nav-section-title">{section.section}</div>
              {section.items.map((item) => (
                <NavLink
                  key={item.to}
                  to={item.to}
                  className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}
                  onClick={() => setSidebarOpen(false)}
                >
                  <span className="nav-link-icon">{item.icon}</span>
                  {item.label}
                </NavLink>
              ))}
            </div>
          ))}
        </nav>

        <div className="sidebar-footer">
          <div className="sidebar-user">
            <div className="sidebar-user-avatar">{initials}</div>
            <div className="sidebar-user-info">
              <div className="sidebar-user-name">{userName}</div>
              <div className="sidebar-user-role">Aktif</div>
            </div>
            <button className="sidebar-logout-btn" onClick={logout} title="Çıkış Yap">
              <FiLogOut />
            </button>
          </div>
        </div>
      </aside>

      {/* Main */}
      <main className="main-content">
        <header className="topbar">
          <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
            <button className="mobile-menu-btn" onClick={() => setSidebarOpen(true)}>
              {sidebarOpen ? <FiX /> : <FiMenu />}
            </button>
            <h2 className="topbar-title">{currentTitle}</h2>
          </div>
          <div className="topbar-actions">
            <div className="notification-wrapper">
              <button 
                className="topbar-btn" 
                title="Bildirimler"
                onClick={() => setShowNotifications(!showNotifications)}
              >
                <FiBell />
                {notifications.length > 0 && <span className="notification-badge" />}
              </button>
              
              {showNotifications && (
                <div className="notification-dropdown">
                  <div className="notification-dropdown-header">Bildirimler</div>
                  <div className="notification-dropdown-list">
                    {notifications.length === 0 ? (
                      <div className="notification-empty">Bildirim bulunmuyor.</div>
                    ) : (
                      notifications.map((notif, idx) => (
                        <div key={idx} className="notification-item">
                          {notif.content}
                        </div>
                      ))
                    )}
                  </div>
                </div>
              )}
            </div>
          </div>
        </header>

        <div className="page-content fade-in">
          <Outlet />
        </div>
      </main>
    </div>
  );
}
