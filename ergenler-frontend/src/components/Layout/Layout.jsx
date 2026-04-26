import { useState } from 'react';
import { NavLink, Outlet, useLocation } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import {
  FiHome, FiPackage, FiShoppingCart, FiDollarSign,
  FiStar, FiLogOut, FiMenu, FiX, FiUser
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
      { to: '/advertisements', label: 'İlanlar', icon: <FiStar /> },
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
  '/advertisements': 'İlan Yönetimi',
  '/accounting': 'Muhasebe',
  '/profile': 'Profil Ayarları',
};

export default function Layout() {
  const { user, logout } = useAuth();
  const location = useLocation();
  const [sidebarOpen, setSidebarOpen] = useState(false);

  const currentTitle = pageTitles[location.pathname] || 'Portal';
  const initials = user?.fullName
    ? user.fullName.split(' ').map((n) => n[0]).join('').toUpperCase()
    : 'U';

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
                {user?.fullName || 'Ebatlama Yönetimi'}
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
              <div className="sidebar-user-name">{user?.fullName || 'Kullanıcı'}</div>
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
          <div className="topbar-actions" />
        </header>

        <div className="page-content fade-in">
          <Outlet />
        </div>
      </main>
    </div>
  );
}
