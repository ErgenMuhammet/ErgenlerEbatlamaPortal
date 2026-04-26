import { useState, useEffect } from 'react';
import { orderService } from '../../services/api';
import { FiPlus, FiCheck, FiEdit2, FiTrash2, FiShoppingCart, FiX } from 'react-icons/fi';
import './Orders.css';

export default function Orders() {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filter, setFilter] = useState('all'); // all | active | done
  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState({
    orderName: '',
    countOfMdf: 0,
    countOfBackPanel: 0,
    metreOfPvcBand: 0,
    costOfOrder: 0,
  });

  useEffect(() => {
    loadOrders();
  }, []);

  const loadOrders = async () => {
    setLoading(true);
    try {
      const res = await orderService.getAllOrders();
      setOrders(res.data?.data || []);
    } catch {
      setOrders([]);
    } finally {
      setLoading(false);
    }
  };

  const handleAddOrder = async () => {
    try {
      await orderService.addOrder(formData);
      setShowModal(false);
      setFormData({ orderName: '', countOfMdf: 0, countOfBackPanel: 0, metreOfPvcBand: 0, costOfOrder: 0 });
      loadOrders();
    } catch (err) {
      console.error(err);
    }
  };

  const handleMarkDone = async (id) => {
    try {
      await orderService.markOrderDone(id);
      loadOrders();
    } catch (err) {
      console.error(err);
    }
  };

  const handleDelete = async (id) => {
    if (!confirm('Bu siparişi silmek istediğinize emin misiniz?')) return;
    try {
      await orderService.deleteOrder(id);
      loadOrders();
    } catch (err) {
      console.error(err);
    }
  };

  const filteredOrders = orders.filter((o) => {
    if (filter === 'active') return !o.isDone;
    if (filter === 'done') return o.isDone;
    return true;
  });

  return (
    <div className="orders-page">
      {/* Toolbar */}
      <div className="orders-toolbar">
        <div className="orders-filter-group">
          {[
            { key: 'all', label: 'Tümü' },
            { key: 'active', label: 'Aktif' },
            { key: 'done', label: 'Tamamlanan' },
          ].map((f) => (
            <button
              key={f.key}
              className={`filter-btn ${filter === f.key ? 'active' : ''}`}
              onClick={() => setFilter(f.key)}
            >
              {f.label}
            </button>
          ))}
        </div>
        <button className="add-btn" onClick={() => setShowModal(true)}>
          <FiPlus /> Yeni Sipariş
        </button>
      </div>

      {/* Orders Grid */}
      {loading ? (
        <div className="loading-container">
          <div className="loading-spinner" />
        </div>
      ) : filteredOrders.length === 0 ? (
        <div className="empty-state">
          <div className="empty-state-icon"><FiShoppingCart /></div>
          <p>Sipariş bulunamadı</p>
        </div>
      ) : (
        <div className="orders-grid">
          {filteredOrders.map((order) => (
            <div key={order.id} className="order-card slide-up">
              <div className="order-done-overlay">
                {order.isDone ? (
                  <span className="badge badge-success">✓ Tamamlandı</span>
                ) : (
                  <span className="badge badge-warning">Devam Ediyor</span>
                )}
              </div>

              <div className="order-card-header">
                <div>
                  <div className="order-card-name">{order.orderName || 'İsimsiz Sipariş'}</div>
                  <div className="order-card-date">
                    {order.orderDate ? new Date(order.orderDate).toLocaleDateString('tr-TR') : '-'}
                  </div>
                </div>
              </div>

              <div className="order-card-body">
                <div className="order-detail">
                  <span className="order-detail-label">MDF Adet</span>
                  <span className="order-detail-value">{order.countOfMdf ?? 0}</span>
                </div>
                <div className="order-detail">
                  <span className="order-detail-label">Arka Panel</span>
                  <span className="order-detail-value">{order.countOfBackPanel ?? 0}</span>
                </div>
                <div className="order-detail">
                  <span className="order-detail-label">PVC Bant (m)</span>
                  <span className="order-detail-value">{order.metreOfPvcBand ?? 0}</span>
                </div>
                <div className="order-detail">
                  <span className="order-detail-label">Maliyet</span>
                  <span className="order-detail-value">₺{(order.costOfOrder || 0).toLocaleString('tr-TR')}</span>
                </div>
              </div>

              <div className="order-card-footer">
                {!order.isDone && (
                  <button className="order-btn done" onClick={() => handleMarkDone(order.id)}>
                    <FiCheck /> Tamamla
                  </button>
                )}
                <button className="order-btn edit">
                  <FiEdit2 /> Düzenle
                </button>
                <button className="order-btn remove" onClick={() => handleDelete(order.id)}>
                  <FiTrash2 /> Sil
                </button>
              </div>
            </div>
          ))}
        </div>
      )}

      {/* Add Order Modal */}
      {showModal && (
        <div className="modal-overlay" onClick={() => setShowModal(false)}>
          <div className="modal" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h3>Yeni Sipariş Ekle</h3>
              <button className="modal-close" onClick={() => setShowModal(false)}>
                <FiX />
              </button>
            </div>
            <div className="modal-body">
              <div className="form-group">
                <label className="form-label">Sipariş Adı</label>
                <input
                  className="form-input"
                  type="text"
                  placeholder="Sipariş adı"
                  value={formData.orderName}
                  onChange={(e) => setFormData({ ...formData, orderName: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label className="form-label">MDF Adet</label>
                <input
                  className="form-input"
                  type="number"
                  value={formData.countOfMdf}
                  onChange={(e) => setFormData({ ...formData, countOfMdf: Number(e.target.value) })}
                />
              </div>
              <div className="form-group">
                <label className="form-label">Arka Panel Adet</label>
                <input
                  className="form-input"
                  type="number"
                  value={formData.countOfBackPanel}
                  onChange={(e) => setFormData({ ...formData, countOfBackPanel: Number(e.target.value) })}
                />
              </div>
              <div className="form-group">
                <label className="form-label">PVC Bant (metre)</label>
                <input
                  className="form-input"
                  type="number"
                  step="0.1"
                  value={formData.metreOfPvcBand}
                  onChange={(e) => setFormData({ ...formData, metreOfPvcBand: Number(e.target.value) })}
                />
              </div>
              <div className="form-group">
                <label className="form-label">Maliyet (₺)</label>
                <input
                  className="form-input"
                  type="number"
                  step="0.01"
                  value={formData.costOfOrder}
                  onChange={(e) => setFormData({ ...formData, costOfOrder: Number(e.target.value) })}
                />
              </div>
            </div>
            <div className="modal-footer">
              <button className="btn-cancel" onClick={() => setShowModal(false)}>İptal</button>
              <button className="btn-save" onClick={handleAddOrder}>Kaydet</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
