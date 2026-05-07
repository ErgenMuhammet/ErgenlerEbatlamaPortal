import { useState, useEffect } from 'react';
import { accountingService } from '../../services/api';
import {
  FiPlus, FiTrendingUp, FiTrendingDown,
  FiFileText, FiCheck, FiX, FiZap, FiDroplet, FiThermometer,
  FiCreditCard, FiDollarSign, FiMoreHorizontal, FiSearch
} from 'react-icons/fi';
import './Accounting.css';

export default function Accounting() {
  const [electricInvoices, setElectricInvoices] = useState([]);
  const [waterInvoices, setWaterInvoices] = useState([]);
  const [naturalGasInvoices, setNaturalGasInvoices] = useState([]);
  const [otherInvoices, setOtherInvoices] = useState([]);
  const [allInvoices, setAllInvoices] = useState([]);

  const [creditCardExpenses, setCreditCardExpenses] = useState([]);
  const [cashExpenses, setCashExpenses] = useState([]);
  const [otherExpenses, setOtherExpenses] = useState([]);
  const [allExpenses, setAllExpenses] = useState([]);

  const [creditCardIncomes, setCreditCardIncomes] = useState([]);
  const [cashIncomes, setCashIncomes] = useState([]);
  const [otherIncomes, setOtherIncomes] = useState([]);
  const [allIncomes, setAllIncomes] = useState([]);

  const [pastInvoices, setPastInvoices] = useState([]);
  const [profitLoss, setProfitLoss] = useState({ totalProfit: 0, totalLoss: 0, lastSituation: 0 });
  const [loading, setLoading] = useState(true);
  const [modal, setModal] = useState(null);
  const [formData, setFormData] = useState({});
  const [activeInvoiceTab, setActiveInvoiceTab] = useState('all');
  const [activeExpenseTab, setActiveExpenseTab] = useState('all');
  const [activeIncomeTab, setActiveIncomeTab] = useState('all');
  const [searchQuery, setSearchQuery] = useState('');

  useEffect(() => { loadAll(); }, []);

  const loadAll = async () => {
    setLoading(true);
    try {
      const [inv, inc, exp, pl] = await Promise.allSettled([
        accountingService.getAllInvoices(),
        accountingService.getAllIncomes(),
        accountingService.getAllExpenses(),
        accountingService.getProfitLossSituation(),
      ]);
      if (inv.status === 'fulfilled') {
        const d = inv.value.data;
        // Backend PascalCase veya camelCase gönderebilir, her ikisini de kontrol et
        const electric = d?.electricInvoices || d?.ElectricInvoices || [];
        const water = d?.waterInvoices || d?.WaterInvoices || [];
        const naturalGas = d?.naturalGasInvoices || d?.NaturalGasInvoices || [];
        const other = d?.otherInvoices || d?.OtherInvoices || [];

        setElectricInvoices(electric);
        setWaterInvoices(water);
        setNaturalGasInvoices(naturalGas);
        setOtherInvoices(other);
        
        setAllInvoices([...electric, ...water, ...naturalGas, ...other]);
      }
      if (inc.status === 'fulfilled') {
        const d = inc.value.data;
        setCreditCardIncomes(d?.creditCardIncomes || []);
        setCashIncomes(d?.cashIncomes || []);
        setOtherIncomes(d?.otherIncomes || []);
        setAllIncomes([...(d?.creditCardIncomes || []), ...(d?.cashIncomes || []), ...(d?.otherIncomes || [])]);
      } else { setAllIncomes([]); }
      if (exp.status === 'fulfilled') {
        const d = exp.value.data;
        setCreditCardExpenses(d?.creditCardExpenses || []);
        setCashExpenses(d?.cashExpenses || []);
        setOtherExpenses(d?.otherExpenses || []);
        setAllExpenses([...(d?.creditCardExpenses || []), ...(d?.cashExpenses || []), ...(d?.otherExpenses || [])]);
      } else { setAllExpenses([]); }
      setProfitLoss(pl.status === 'fulfilled' && pl.value.data?.data ? pl.value.data.data : { totalProfit: 0, totalLoss: 0, lastSituation: 0 });
    } catch { /* fallback */ } finally { setLoading(false); }
  };

  const handleSave = async () => {
    try {
      if (modal === 'income') await accountingService.addIncome(formData);
      if (modal === 'expense') await accountingService.addExpense(formData);
      if (modal === 'invoice') await accountingService.addInvoice(formData);
      setModal(null); setFormData({}); loadAll();
    } catch (err) { console.error(err); }
  };

  const handleOpenPastInvoices = async () => {
    try {
      const res = await accountingService.getPayedInvoices();
      setPastInvoices(res.data?.invoices || []);
    } catch { setPastInvoices([]); }
    setSearchQuery('');
    setModal('pastInvoices');
  };

  const handlePayInvoice = async (id) => {
    try { await accountingService.payInvoice(id); loadAll(); } catch (err) { console.error(err); }
  };

  const filterBySearch = (list, query) => {
    if (!query.trim()) return list;
    const q = query.toLowerCase();
    return list.filter(item => {
      const desc = (item.description || item.name || item.Name || item.invoiceName || item.InvoiceName || '').toLowerCase();
      const no = (item.invoicesNo || item.InvoicesNo || item.invoiceNo || item.InvoiceNo || '').toLowerCase();
      return desc.includes(q) || no.includes(q);
    });
  };

  const getActiveInvoiceList = () => filterBySearch({ all: allInvoices, electric: electricInvoices, water: waterInvoices, naturalGas: naturalGasInvoices, other: otherInvoices }[activeInvoiceTab] || [], searchQuery);
  const getActiveExpenseList = () => filterBySearch({ all: allExpenses, creditCard: creditCardExpenses, cash: cashExpenses, other: otherExpenses }[activeExpenseTab] || [], searchQuery);
  const getActiveIncomeList = () => filterBySearch({ all: allIncomes, creditCard: creditCardIncomes, cash: cashIncomes, other: otherIncomes }[activeIncomeTab] || [], searchQuery);
  const getFilteredPastInvoices = () => filterBySearch(pastInvoices, searchQuery);

  const paymentTabs = [
    { key: 'creditCard', label: 'Kredi Kartı', icon: <FiCreditCard />, color: '#a78bfa' },
    { key: 'cash', label: 'Nakit', icon: <FiDollarSign />, color: '#34d399' },
    { key: 'other', label: 'Diğer', icon: <FiMoreHorizontal />, color: '#94a3b8' },
  ];

  const renderInvoiceItem = (inv, showPayBtn = true) => (
    <div key={inv.id || inv.Id} className="invoice-item">
      <div className="invoice-info">
        <span className="invoice-name">{inv.name || inv.Name || inv.invoiceName || inv.InvoiceName || 'İsimsiz'}</span>
        <span className="invoice-no">No: {inv.invoicesNo || inv.InvoicesNo || inv.invoiceNo || inv.InvoiceNo || '-'}</span>
        <span className="invoice-no" style={{ marginTop: '0.25rem', color: 'var(--text-secondary)' }}>
          Son Ödeme: {(inv.lastPaymentDate || inv.LastPaymentDate) ? new Date(inv.lastPaymentDate || inv.LastPaymentDate).toLocaleDateString('tr-TR') : '-'}
        </span>
        {inv.invoiceType && (
          <span className={`invoice-type-badge invoice-type-${(inv.invoiceType || '').toLowerCase()}`}>
            {inv.invoiceType === 'Electric' && '⚡ Elektrik'}
            {inv.invoiceType === 'Water' && '💧 Su'}
            {inv.invoiceType === 'NaturalGas' && '🔥 Doğalgaz'}
            {inv.invoiceType === 'Other' && '📋 Diğer'}
          </span>
        )}
      </div>
      <div className="invoice-actions">
        <span className="invoice-amount">₺{(inv.price || inv.Price || inv.cost || inv.Cost || 0).toLocaleString('tr-TR')}</span>
        {showPayBtn && !(inv.beenPaid || inv.BeenPaid) && <button className="pay-btn" onClick={() => handlePayInvoice(inv.id || inv.Id)}><FiCheck /> Öde</button>}
        {((!showPayBtn) || (inv.beenPaid || inv.BeenPaid)) && <span className="paid-badge">✓ Ödendi</span>}
      </div>
    </div>
  );

  const renderAmountItem = (item, type) => (
    <div key={item.id} className="invoice-item">
      <div className="invoice-info">
        <span className="invoice-name">{item.description || '-'}</span>
        <span className="invoice-no">{(type === 'income' ? item.incomeDate : item.expenseDate) ? new Date(type === 'income' ? item.incomeDate : item.expenseDate).toLocaleDateString('tr-TR') : '-'}</span>
        {(item.incomeType || item.expenseType) && (
          <span className={`invoice-type-badge expense-type-${((item.incomeType || item.expenseType) || '').toLowerCase()}`}>
            {(item.incomeType || item.expenseType) === 'CreditCard' && '💳 Kredi Kartı'}
            {(item.incomeType || item.expenseType) === 'Cash' && '💵 Nakit'}
            {(item.incomeType || item.expenseType) === 'Other' && '📋 Diğer'}
          </span>
        )}
      </div>
      <span className="invoice-amount" style={{ color: type === 'income' ? 'var(--accent-emerald)' : 'var(--accent-rose)' }}>
        {type === 'income' ? '+' : '-'}₺{(item.amount || 0).toLocaleString('tr-TR')}
      </span>
    </div>
  );

  const renderCategoryTabs = (tabs, activeTab, setTab, counts) => (
    <div className="invoice-tabs">
      {tabs.map(t => (
        <button key={t.key} className={`invoice-tab-btn ${activeTab === t.key ? 'active' : ''}`} style={{ '--tab-color': t.color }} onClick={() => setTab(t.key)}>
          <span className="invoice-tab-icon">{t.icon}</span>
          <span className="invoice-tab-label">{t.label}</span>
          <span className="invoice-tab-count">{counts[t.key] || 0}</span>
        </button>
      ))}
    </div>
  );

  if (loading) return <div className="loading-container"><div className="loading-spinner" /></div>;

  return (
    <div className="accounting-page">
      {/* Summary */}
      <div className="accounting-summary">
        <div className="summary-card slide-up">
          <div className="summary-card-label">Toplam Gelir</div>
          <div className="summary-card-value income">₺{(profitLoss.totalProfit || 0).toLocaleString('tr-TR')}</div>
        </div>
        <div className="summary-card slide-up" style={{ animationDelay: '0.1s' }}>
          <div className="summary-card-label">Toplam Gider</div>
          <div className="summary-card-value expense">₺{(profitLoss.totalLoss || 0).toLocaleString('tr-TR')}</div>
        </div>
        <div className="summary-card slide-up" style={{ animationDelay: '0.2s' }}>
          <div className="summary-card-label">Net Durum</div>
          <div className="summary-card-value net">₺{((profitLoss.totalProfit || 0) - (profitLoss.totalLoss || 0)).toLocaleString('tr-TR')}</div>
        </div>
      </div>

      {/* Invoices */}
      <div className="accounting-section">
        <div className="accounting-section-header">
          <span className="accounting-section-title"><FiFileText className="icon" /> Faturalar</span>
          <div style={{ display: 'flex', gap: '0.5rem', flexWrap: 'wrap' }}>
            <button className="add-btn" style={{ backgroundColor: 'var(--surface-light)', color: 'var(--text)' }} onClick={() => { setActiveInvoiceTab('all'); setModal('viewInvoices'); }}>Faturaları Görüntüle</button>
            <button className="add-btn" style={{ backgroundColor: 'var(--surface-light)', color: 'var(--text)' }} onClick={handleOpenPastInvoices}>Ödenmiş Faturalar</button>
            <button className="add-btn" onClick={() => { setFormData({}); setModal('invoice'); }}><FiPlus /> Fatura Ekle</button>
          </div>
        </div>
        <div className="invoice-list">
          {allInvoices.length === 0 ? <div className="empty-state"><p>Henüz fatura yok</p></div> : allInvoices.map(inv => renderInvoiceItem(inv, true))}
        </div>
      </div>

      {/* Income & Expense side by side */}
      <div className="accounting-columns">
        {/* Incomes */}
        <div className="accounting-section">
          <div className="accounting-section-header">
            <span className="accounting-section-title"><FiTrendingUp className="icon" /> Gelirler</span>
            <div style={{ display: 'flex', gap: '0.5rem' }}>
              <button className="add-btn" style={{ backgroundColor: 'var(--surface-light)', color: 'var(--text)' }} onClick={() => { setActiveIncomeTab('all'); setModal('viewIncomes'); }}>Gelirleri Görüntüle</button>
              <button className="add-btn" onClick={() => { setFormData({}); setModal('income'); }}><FiPlus /> Gelir Ekle</button>
            </div>
          </div>
          <div className="invoice-list">
            {allIncomes.length === 0 ? <div className="empty-state"><p>Kayıt yok</p></div> : allIncomes.map(inc => renderAmountItem(inc, 'income'))}
          </div>
        </div>

        {/* Expenses */}
        <div className="accounting-section">
          <div className="accounting-section-header">
            <span className="accounting-section-title"><FiTrendingDown className="icon" /> Giderler</span>
            <div style={{ display: 'flex', gap: '0.5rem' }}>
              <button className="add-btn" style={{ backgroundColor: 'var(--surface-light)', color: 'var(--text)' }} onClick={() => { setActiveExpenseTab('all'); setModal('viewExpenses'); }}>Giderleri Görüntüle</button>
              <button className="add-btn" onClick={() => { setFormData({}); setModal('expense'); }}><FiPlus /> Gider Ekle</button>
            </div>
          </div>
          <div className="invoice-list">
            {allExpenses.length === 0 ? <div className="empty-state"><p>Kayıt yok</p></div> : allExpenses.map(exp => renderAmountItem(exp, 'expense'))}
          </div>
        </div>
      </div>

      {/* Modals */}
      {modal && (
        <div className="modal-overlay" onClick={() => setModal(null)}>
          <div className={`modal ${['viewInvoices', 'viewExpenses', 'viewIncomes', 'pastInvoices'].includes(modal) ? 'modal-xl' : ''}`} onClick={e => e.stopPropagation()}>
            <div className="modal-header">
              <h3>
                {modal === 'income' && 'Gelir Ekle'}{modal === 'expense' && 'Gider Ekle'}{modal === 'invoice' && 'Fatura Ekle'}
                {modal === 'pastInvoices' && 'Ödenmiş Faturalar'}{modal === 'viewInvoices' && 'Faturalar'}
                {modal === 'viewExpenses' && 'Giderler'}{modal === 'viewIncomes' && 'Gelirler'}
              </h3>
              <button className="modal-close" onClick={() => setModal(null)}><FiX /></button>
            </div>
            <div className="modal-body">
              {/* View Invoices */}
              {modal === 'viewInvoices' && (
                <>
                  {renderCategoryTabs(
                    [
                      { key: 'all', label: 'Tümü', icon: <FiSearch />, color: 'var(--accent-primary)' },
                      { key: 'electric', label: 'Elektrik', icon: <FiZap />, color: '#facc15' }, 
                      { key: 'water', label: 'Su', icon: <FiDroplet />, color: '#38bdf8' }, 
                      { key: 'naturalGas', label: 'Doğalgaz', icon: <FiThermometer />, color: '#f97316' },
                      { key: 'other', label: 'Diğer', icon: <FiMoreHorizontal />, color: '#94a3b8' }
                    ],
                    activeInvoiceTab, setActiveInvoiceTab,
                    { all: allInvoices.length, electric: electricInvoices.length, water: waterInvoices.length, naturalGas: naturalGasInvoices.length, other: otherInvoices.length }
                  )}
                  <div className="search-box"><FiSearch className="search-icon" /><input className="search-input" type="text" placeholder="Fatura adı veya no ile ara..." value={searchQuery} onChange={e => setSearchQuery(e.target.value)} /></div>
                  <div className="invoice-list" style={{ maxHeight: '50vh', overflowY: 'auto' }}>
                    {getActiveInvoiceList().length === 0 ? <div className="empty-state"><p>Fatura bulunamadı</p></div> : getActiveInvoiceList().map(inv => renderInvoiceItem(inv, true))}
                  </div>
                </>
              )}

              {/* View Incomes */}
              {modal === 'viewIncomes' && (
                <>
                  {renderCategoryTabs(
                    [
                      { key: 'all', label: 'Tümü', icon: <FiSearch />, color: 'var(--accent-primary)' },
                      ...paymentTabs
                    ], 
                    activeIncomeTab, setActiveIncomeTab, 
                    { all: allIncomes.length, creditCard: creditCardIncomes.length, cash: cashIncomes.length, other: otherIncomes.length }
                  )}
                  <div className="search-box"><FiSearch className="search-icon" /><input className="search-input" type="text" placeholder="Açıklamaya göre ara..." value={searchQuery} onChange={e => setSearchQuery(e.target.value)} /></div>
                  <div className="invoice-list" style={{ maxHeight: '50vh', overflowY: 'auto' }}>
                    {getActiveIncomeList().length === 0 ? <div className="empty-state"><p>Gelir bulunamadı</p></div> : getActiveIncomeList().map(inc => renderAmountItem(inc, 'income'))}
                  </div>
                </>
              )}

              {/* View Expenses */}
              {modal === 'viewExpenses' && (
                <>
                  {renderCategoryTabs(
                    [
                      { key: 'all', label: 'Tümü', icon: <FiSearch />, color: 'var(--accent-primary)' },
                      ...paymentTabs
                    ], 
                    activeExpenseTab, setActiveExpenseTab, 
                    { all: allExpenses.length, creditCard: creditCardExpenses.length, cash: cashExpenses.length, other: otherExpenses.length }
                  )}
                  <div className="search-box"><FiSearch className="search-icon" /><input className="search-input" type="text" placeholder="Açıklamaya göre ara..." value={searchQuery} onChange={e => setSearchQuery(e.target.value)} /></div>
                  <div className="invoice-list" style={{ maxHeight: '50vh', overflowY: 'auto' }}>
                    {getActiveExpenseList().length === 0 ? <div className="empty-state"><p>Gider bulunamadı</p></div> : getActiveExpenseList().map(exp => renderAmountItem(exp, 'expense'))}
                  </div>
                </>
              )}

              {/* Past Invoices */}
              {modal === 'pastInvoices' && (
                <>
                  <div className="search-box"><FiSearch className="search-icon" /><input className="search-input" type="text" placeholder="Fatura adı veya no ile ara..." value={searchQuery} onChange={e => setSearchQuery(e.target.value)} /></div>
                  <div className="invoice-list" style={{ maxHeight: '55vh', overflowY: 'auto' }}>
                    {getFilteredPastInvoices().length === 0 ? <div className="empty-state"><p>Ödenmiş fatura yok</p></div> : getFilteredPastInvoices().map(inv => renderInvoiceItem(inv, false))}
                  </div>
                </>
              )}

              {/* Add Invoice */}
              {modal === 'invoice' && (
                <>
                  <div className="form-group"><label className="form-label">Fatura Adı</label><input className="form-input" type="text" placeholder="Fatura adı" value={formData.invoiceName || ''} onChange={e => setFormData({ ...formData, invoiceName: e.target.value })} /></div>
                  <div className="form-group"><label className="form-label">Fatura No</label><input className="form-input" type="text" placeholder="Fatura no" value={formData.invoiceNo || ''} onChange={e => setFormData({ ...formData, invoiceNo: e.target.value })} /></div>
                  <div className="form-group">
                    <label className="form-label">Fatura Kategorisi</label>
                    <div className="invoice-type-selector">
                      {[
                        { value: 'Electric', label: 'Elektrik', icon: <FiZap />, color: '#facc15' }, 
                        { value: 'Water', label: 'Su', icon: <FiDroplet />, color: '#38bdf8' }, 
                        { value: 'NaturalGas', label: 'Doğalgaz', icon: <FiThermometer />, color: '#f97316' },
                        { value: 'Other', label: 'Diğer', icon: <FiMoreHorizontal />, color: '#94a3b8' }
                      ].map(t => (
                        <button key={t.value} type="button" className={`invoice-type-option ${formData.invoiceType === t.value ? 'selected' : ''}`} style={{ '--type-color': t.color }} onClick={() => setFormData({ ...formData, invoiceType: t.value })}>
                          <span className="type-icon">{t.icon}</span><span>{t.label}</span>
                        </button>
                      ))}
                    </div>
                  </div>
                  <div className="form-group"><label className="form-label">Tutar (₺)</label><input className="form-input" type="number" step="0.01" value={formData.cost || ''} onChange={e => setFormData({ ...formData, cost: Number(e.target.value) })} /></div>
                  <div className="form-group"><label className="form-label">Son Ödeme Tarihi</label><input className="form-input" type="date" value={formData.lastPaymentDate || ''} onChange={e => setFormData({ ...formData, lastPaymentDate: e.target.value })} /></div>
                </>
              )}

              {/* Add Income */}
              {modal === 'income' && (
                <>
                  <div className="form-group"><label className="form-label">Açıklama</label><input className="form-input" type="text" placeholder="Açıklama" value={formData.description || ''} onChange={e => setFormData({ ...formData, description: e.target.value })} /></div>
                  <div className="form-group"><label className="form-label">Tutar (₺)</label><input className="form-input" type="number" step="0.01" value={formData.amount || ''} onChange={e => setFormData({ ...formData, amount: Number(e.target.value) })} /></div>
                  <div className="form-group"><label className="form-label">Gelir Tarihi</label><input className="form-input" type="date" value={formData.incomeDate || ''} onChange={e => setFormData({ ...formData, incomeDate: e.target.value })} /></div>
                  <div className="form-group">
                    <label className="form-label">Ödeme Tipi</label>
                    <div className="invoice-type-selector">
                      {[{ value: 'CreditCard', label: 'Kredi Kartı', icon: <FiCreditCard />, color: '#a78bfa' }, { value: 'Cash', label: 'Nakit', icon: <FiDollarSign />, color: '#34d399' }, { value: 'Other', label: 'Diğer', icon: <FiMoreHorizontal />, color: '#94a3b8' }].map(t => (
                        <button key={t.value} type="button" className={`invoice-type-option ${formData.incomeType === t.value ? 'selected' : ''}`} style={{ '--type-color': t.color }} onClick={() => setFormData({ ...formData, incomeType: t.value })}>
                          <span className="type-icon">{t.icon}</span><span>{t.label}</span>
                        </button>
                      ))}
                    </div>
                  </div>
                </>
              )}

              {/* Add Expense */}
              {modal === 'expense' && (
                <>
                  <div className="form-group"><label className="form-label">Açıklama</label><input className="form-input" type="text" placeholder="Açıklama" value={formData.description || ''} onChange={e => setFormData({ ...formData, description: e.target.value })} /></div>
                  <div className="form-group"><label className="form-label">Tutar (₺)</label><input className="form-input" type="number" step="0.01" value={formData.amount || ''} onChange={e => setFormData({ ...formData, amount: Number(e.target.value) })} /></div>
                  <div className="form-group"><label className="form-label">Gider Tarihi</label><input className="form-input" type="date" value={formData.expenseDate || ''} onChange={e => setFormData({ ...formData, expenseDate: e.target.value })} /></div>
                  <div className="form-group">
                    <label className="form-label">Ödeme Tipi</label>
                    <div className="invoice-type-selector">
                      {[{ value: 'CreditCard', label: 'Kredi Kartı', icon: <FiCreditCard />, color: '#a78bfa' }, { value: 'Cash', label: 'Nakit', icon: <FiDollarSign />, color: '#34d399' }, { value: 'Other', label: 'Diğer', icon: <FiMoreHorizontal />, color: '#94a3b8' }].map(t => (
                        <button key={t.value} type="button" className={`invoice-type-option ${formData.expenseType === t.value ? 'selected' : ''}`} style={{ '--type-color': t.color }} onClick={() => setFormData({ ...formData, expenseType: t.value })}>
                          <span className="type-icon">{t.icon}</span><span>{t.label}</span>
                        </button>
                      ))}
                    </div>
                  </div>
                </>
              )}
            </div>
            <div className="modal-footer">
              <button className="btn-cancel" onClick={() => setModal(null)}>Kapat</button>
              {!['pastInvoices', 'viewInvoices', 'viewExpenses', 'viewIncomes'].includes(modal) && <button className="btn-save" onClick={handleSave}>Kaydet</button>}
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
