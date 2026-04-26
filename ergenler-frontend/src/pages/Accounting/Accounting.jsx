import { useState, useEffect } from 'react';
import { accountingService } from '../../services/api';
import {
  FiPlus, FiDollarSign, FiTrendingUp, FiTrendingDown,
  FiFileText, FiCheck, FiX
} from 'react-icons/fi';
import './Accounting.css';

export default function Accounting() {
  const [invoices, setInvoices] = useState([]);
  const [incomes, setIncomes] = useState([]);
  const [expenses, setExpenses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [modal, setModal] = useState(null); // 'income' | 'expense' | 'invoice' | null
  const [formData, setFormData] = useState({});

  useEffect(() => {
    loadAll();
  }, []);

  const loadAll = async () => {
    setLoading(true);
    try {
      const [inv, inc, exp] = await Promise.allSettled([
        accountingService.getAllInvoices(),
        accountingService.getAllIncomes(),
        accountingService.getAllExpenses(),
      ]);
      setInvoices(inv.status === 'fulfilled' ? (inv.value.data?.data || []) : []);
      setIncomes(inc.status === 'fulfilled' ? (inc.value.data?.data || []) : []);
      setExpenses(exp.status === 'fulfilled' ? (exp.value.data?.data || []) : []);
    } catch {
      // fallback
    } finally {
      setLoading(false);
    }
  };

  const totalIncome = incomes.reduce((s, i) => s + (i.amount || 0), 0);
  const totalExpense = expenses.reduce((s, e) => s + (e.amount || 0), 0);
  const netAmount = totalIncome - totalExpense;

  const handleSave = async () => {
    try {
      if (modal === 'income') await accountingService.addIncome(formData);
      if (modal === 'expense') await accountingService.addExpense(formData);
      if (modal === 'invoice') await accountingService.addInvoice(formData);
      setModal(null);
      setFormData({});
      loadAll();
    } catch (err) {
      console.error(err);
    }
  };

  const handlePayInvoice = async (id) => {
    try {
      await accountingService.payInvoice(id);
      loadAll();
    } catch (err) {
      console.error(err);
    }
  };

  if (loading) {
    return (
      <div className="loading-container">
        <div className="loading-spinner" />
      </div>
    );
  }

  return (
    <div className="accounting-page">
      {/* Summary */}
      <div className="accounting-summary">
        <div className="summary-card slide-up">
          <div className="summary-card-label">Toplam Gelir</div>
          <div className="summary-card-value income">₺{totalIncome.toLocaleString('tr-TR')}</div>
        </div>
        <div className="summary-card slide-up" style={{ animationDelay: '0.1s' }}>
          <div className="summary-card-label">Toplam Gider</div>
          <div className="summary-card-value expense">₺{totalExpense.toLocaleString('tr-TR')}</div>
        </div>
        <div className="summary-card slide-up" style={{ animationDelay: '0.2s' }}>
          <div className="summary-card-label">Net Durum</div>
          <div className="summary-card-value net">₺{netAmount.toLocaleString('tr-TR')}</div>
        </div>
      </div>

      {/* Invoices */}
      <div className="accounting-section">
        <div className="accounting-section-header">
          <span className="accounting-section-title">
            <FiFileText className="icon" /> Faturalar
          </span>
          <button className="add-btn" onClick={() => { setFormData({}); setModal('invoice'); }}>
            <FiPlus /> Fatura Ekle
          </button>
        </div>
        <div className="invoice-list">
          {invoices.length === 0 ? (
            <div className="empty-state"><p>Henüz fatura yok</p></div>
          ) : (
            invoices.map((inv) => (
              <div key={inv.id} className="invoice-item">
                <div className="invoice-info">
                  <span className="invoice-name">{inv.invoiceName || 'İsimsiz'}</span>
                  <span className="invoice-no">No: {inv.invoiceNo || '-'}</span>
                </div>
                <div className="invoice-actions">
                  <span className="invoice-amount">₺{(inv.cost || 0).toLocaleString('tr-TR')}</span>
                  {inv.beenPaid ? (
                    <span className="paid-badge">✓ Ödendi</span>
                  ) : (
                    <button className="pay-btn" onClick={() => handlePayInvoice(inv.id)}>
                      <FiCheck /> Öde
                    </button>
                  )}
                </div>
              </div>
            ))
          )}
        </div>
      </div>

      {/* Income & Expense side by side */}
      <div className="accounting-columns">
        {/* Incomes */}
        <div className="accounting-section">
          <div className="accounting-section-header">
            <span className="accounting-section-title">
              <FiTrendingUp className="icon" /> Gelirler
            </span>
            <button className="add-btn" onClick={() => { setFormData({}); setModal('income'); }}>
              <FiPlus /> Gelir Ekle
            </button>
          </div>
          <div className="invoice-list">
            {incomes.length === 0 ? (
              <div className="empty-state"><p>Kayıt yok</p></div>
            ) : (
              incomes.map((inc) => (
                <div key={inc.id} className="invoice-item">
                  <div className="invoice-info">
                    <span className="invoice-name">{inc.description || '-'}</span>
                    <span className="invoice-no">
                      {inc.incomeDate ? new Date(inc.incomeDate).toLocaleDateString('tr-TR') : '-'}
                    </span>
                  </div>
                  <span className="invoice-amount" style={{ color: 'var(--accent-emerald)' }}>
                    +₺{(inc.amount || 0).toLocaleString('tr-TR')}
                  </span>
                </div>
              ))
            )}
          </div>
        </div>

        {/* Expenses */}
        <div className="accounting-section">
          <div className="accounting-section-header">
            <span className="accounting-section-title">
              <FiTrendingDown className="icon" /> Giderler
            </span>
            <button className="add-btn" onClick={() => { setFormData({}); setModal('expense'); }}>
              <FiPlus /> Gider Ekle
            </button>
          </div>
          <div className="invoice-list">
            {expenses.length === 0 ? (
              <div className="empty-state"><p>Kayıt yok</p></div>
            ) : (
              expenses.map((exp) => (
                <div key={exp.id} className="invoice-item">
                  <div className="invoice-info">
                    <span className="invoice-name">{exp.description || '-'}</span>
                    <span className="invoice-no">
                      {exp.expenseDate ? new Date(exp.expenseDate).toLocaleDateString('tr-TR') : '-'}
                    </span>
                  </div>
                  <span className="invoice-amount" style={{ color: 'var(--accent-rose)' }}>
                    -₺{(exp.amount || 0).toLocaleString('tr-TR')}
                  </span>
                </div>
              ))
            )}
          </div>
        </div>
      </div>

      {/* Add Modal */}
      {modal && (
        <div className="modal-overlay" onClick={() => setModal(null)}>
          <div className="modal" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h3>
                {modal === 'income' && 'Gelir Ekle'}
                {modal === 'expense' && 'Gider Ekle'}
                {modal === 'invoice' && 'Fatura Ekle'}
              </h3>
              <button className="modal-close" onClick={() => setModal(null)}>
                <FiX />
              </button>
            </div>
            <div className="modal-body">
              {modal === 'invoice' && (
                <>
                  <div className="form-group">
                    <label className="form-label">Fatura Adı</label>
                    <input
                      className="form-input"
                      type="text"
                      placeholder="Fatura adı"
                      value={formData.invoiceName || ''}
                      onChange={(e) => setFormData({ ...formData, invoiceName: e.target.value })}
                    />
                  </div>
                  <div className="form-group">
                    <label className="form-label">Fatura No</label>
                    <input
                      className="form-input"
                      type="text"
                      placeholder="Fatura no"
                      value={formData.invoiceNo || ''}
                      onChange={(e) => setFormData({ ...formData, invoiceNo: e.target.value })}
                    />
                  </div>
                  <div className="form-group">
                    <label className="form-label">Tutar (₺)</label>
                    <input
                      className="form-input"
                      type="number"
                      step="0.01"
                      value={formData.cost || ''}
                      onChange={(e) => setFormData({ ...formData, cost: Number(e.target.value) })}
                    />
                  </div>
                </>
              )}
              {(modal === 'income' || modal === 'expense') && (
                <>
                  <div className="form-group">
                    <label className="form-label">Açıklama</label>
                    <input
                      className="form-input"
                      type="text"
                      placeholder="Açıklama"
                      value={formData.description || ''}
                      onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                    />
                  </div>
                  <div className="form-group">
                    <label className="form-label">Tutar (₺)</label>
                    <input
                      className="form-input"
                      type="number"
                      step="0.01"
                      value={formData.amount || ''}
                      onChange={(e) => setFormData({ ...formData, amount: Number(e.target.value) })}
                    />
                  </div>
                </>
              )}
            </div>
            <div className="modal-footer">
              <button className="btn-cancel" onClick={() => setModal(null)}>İptal</button>
              <button className="btn-save" onClick={handleSave}>Kaydet</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
