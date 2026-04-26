import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { materialService, orderService, accountingService } from '../../services/api';
import {
  FiPackage, FiShoppingCart, FiDollarSign, FiTrendingUp,
  FiPlus, FiList, FiPieChart
} from 'react-icons/fi';
import './Dashboard.css';

export default function Dashboard() {
  const { user } = useAuth();
  const [stats, setStats] = useState({
    mdfCount: 0,
    orderCount: 0,
    incomeTotal: 0,
    expenseTotal: 0,
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadStats();
  }, []);

  const loadStats = async () => {
    try {
      const [ordersRes, incomesRes, expensesRes] = await Promise.allSettled([
        orderService.getAllOrders(),
        accountingService.getAllIncomes(),
        accountingService.getAllExpenses(),
      ]);

      setStats({
        orderCount: ordersRes.status === 'fulfilled' ? (ordersRes.value.data?.data?.length || 0) : 0,
        incomeTotal: incomesRes.status === 'fulfilled'
          ? (ordersRes.value.data?.data || []).reduce((sum, i) => sum + (i.amount || 0), 0)
          : 0,
        expenseTotal: expensesRes.status === 'fulfilled'
          ? (expensesRes.value.data?.data || []).reduce((sum, e) => sum + (e.amount || 0), 0)
          : 0,
      });
    } catch {
      // Stats will remain at defaults
    } finally {
      setLoading(false);
    }
  };

  const statCards = [
    {
      title: 'Toplam Sipariş',
      value: stats.orderCount,
      icon: <FiShoppingCart />,
      color: 'indigo',
      sub: 'Aktif siparişler',
    },
    {
      title: 'Toplam Gelir',
      value: `₺${stats.incomeTotal.toLocaleString('tr-TR')}`,
      icon: <FiTrendingUp />,
      color: 'emerald',
      sub: 'Bu dönem',
    },
    {
      title: 'Toplam Gider',
      value: `₺${stats.expenseTotal.toLocaleString('tr-TR')}`,
      icon: <FiDollarSign />,
      color: 'rose',
      sub: 'Bu dönem',
    },
    {
      title: 'Net Durum',
      value: `₺${(stats.incomeTotal - stats.expenseTotal).toLocaleString('tr-TR')}`,
      icon: <FiPieChart />,
      color: stats.incomeTotal - stats.expenseTotal >= 0 ? 'cyan' : 'amber',
      sub: 'Kâr / Zarar',
    },
  ];

  return (
    <div className="dashboard">
      {/* Welcome */}
      <div className="dashboard-welcome slide-up">
        <h2>Hoş geldiniz, {user?.fullName || 'Kullanıcı'} 👋</h2>
        <p>Ergenler Ebatlama Portal — işletmenizi tek noktadan yönetin.</p>
      </div>

      {/* Stats */}
      <div className="dashboard-stats">
        {statCards.map((card, i) => (
          <div key={i} className="stat-card slide-up" style={{ animationDelay: `${i * 0.1}s` }}>
            {loading ? (
              <>
                <div className="skeleton skeleton-text" />
                <div className="skeleton skeleton-value" />
              </>
            ) : (
              <>
                <div className="stat-card-header">
                  <span className="stat-card-title">{card.title}</span>
                  <div className={`stat-card-icon ${card.color}`}>{card.icon}</div>
                </div>
                <div className="stat-card-value">{card.value}</div>
                <div className="stat-card-sub">{card.sub}</div>
              </>
            )}
          </div>
        ))}
      </div>

      {/* Quick Actions */}
      <div>
        <h3 className="dashboard-section-title">Hızlı İşlemler</h3>
        <div className="quick-actions">
          <Link to="/stock" className="quick-action-card">
            <span className="quick-action-icon"><FiPackage /></span>
            <span className="quick-action-text">Stok Görüntüle</span>
          </Link>
          <Link to="/orders" className="quick-action-card">
            <span className="quick-action-icon"><FiPlus /></span>
            <span className="quick-action-text">Yeni Sipariş</span>
          </Link>
          <Link to="/accounting" className="quick-action-card">
            <span className="quick-action-icon"><FiDollarSign /></span>
            <span className="quick-action-text">Gelir/Gider Ekle</span>
          </Link>
          <Link to="/advertisements" className="quick-action-card">
            <span className="quick-action-icon"><FiList /></span>
            <span className="quick-action-text">İlanları Gör</span>
          </Link>
        </div>
      </div>
    </div>
  );
}
