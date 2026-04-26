import axios from 'axios';

const API_BASE_URL = '/portal';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor â€” attach JWT token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor â€” handle 401
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// ===== AUTH =====
export const authService = {
  login: (data) => api.post('/login', data),
  register: (data) => api.post('/KayÄ±tOl', data),
  confirmEmail: (userid, token) => api.post(`/ConfirmEmail?userid=${userid}&token=${token}`),
  passwordRecovery: (data) => api.patch('/PasswordRecovery', data),
  validateToken: (userId, token, data) => api.patch(`/ValidateToken?userId=${userId}&token=${token}`, data),
  updateMyProperty: (data) => api.patch('/UpdateMyProperty', data),
  updateMyJobsProperty: (data) => api.patch('/UpdateMyJobsProperty', data),
};

// ===== MATERIALS / STOCK =====
export const materialService = {
  // GET stock
  getMdfStock: () => api.get('/Stok/Mdf'),
  getGlueStock: () => api.get('/Stok/Glue'),
  getBackPanelStock: () => api.get('/Stok/BackPanel'),
  getScraps: () => api.get('/Stok/Scraps'),
  getPvcBandStock: () => api.get('/Stok/PvcBand'),

  // ADD stock
  addMdf: (data) => api.post('/Stok/AddMdf', data),
  addGlue: (data) => api.post('/Stok/AddGlue', data),
  addBackPanel: (data) => api.post('/Stok/AddBackPanel', data),
  addPvcBand: (data) => api.post('/Stok/AddPvcBand', data),
  addScrap: (data) => api.post('/Stok/AddScrap', data),

  // UPDATE stock
  updateMdf: (id, data) => api.put(`/Update/Mdf/${id}`, data),
  updateGlue: (id, data) => api.put(`/Update/Glue/${id}`, data),
  updateBackPanel: (id, data) => api.put(`/Update/BackPanel/${id}`, data),
  updatePvcBand: (id, data) => api.put(`/Update/PvcBand/${id}`, data),
  updateScrap: (id, data) => api.put(`/Update/Scrap/${id}`, data),

  // REDUCE stock
  reduceMdf: (id, data) => api.put(`/Stock/Reduce/Mdf/${id}`, data),
  reduceGlue: (id, data) => api.put(`/Stock/Reduce/Glue/${id}`, data),
  reducePvcBand: (id, data) => api.put(`/Stock/Reduce/PvcBand/${id}`, data),
  reduceScrap: (id, data) => api.put(`/Stock/Reduce/Scraps/${id}`, data),
  reduceBackPanel: (id, data) => api.put(`/Stock/Reduce/BackPanel/${id}`, data),
};

// ===== ORDERS =====
export const orderService = {
  addOrder: (data) => api.post('/order/AddOrder', data),
  getAllOrders: () => api.get('/order/GetAllOrder'),
  deleteOrder: (id) => api.delete(`/order/Delete/${id}`),
  updateOrder: (id, data) => api.put(`/order/Update/${id}`, data),
  markOrderDone: (id) => api.patch(`/order/OrderIsDone/${id}`),
  getMeasurements: (params) => api.get('/order/GetMeasurements', { params }),
};

// ===== ACCOUNTING =====
export const accountingService = {
  // Invoices
  getAllInvoices: () => api.get('/Accounting/GetAllInvoice'),
  getPayedInvoices: () => api.get('/Accounting/GetPayedInvoice'),
  addInvoice: (data) => api.post('/Accounting/AddInvoice', data),
  payInvoice: (id) => api.post(`/Accounting/MarkThePayedToInvoice/${id}`),

  // Expenses
  getAllExpenses: () => api.get('/Accounting/GetAllExpense'),
  addExpense: (data) => api.post('/Accounting/AddExpense', data),
  updateExpense: (id, data) => api.patch(`/Accounting/UpdateExpense/${id}`, data),

  // Incomes
  getAllIncomes: () => api.get('/Accounting/GetAllIncome'),
  addIncome: (data) => api.post('/Accounting/AddIncome', data),
  
  // Profit & Loss
  getProfitLossSituation: () => api.get('/Accounting/GetProfitLossSituation'),
};

// ===== ADVERTISEMENTS =====
export const advertisementService = {
  getAll: () => api.get('/GetAllAdvertisement'),
  getMine: () => api.get('/GetMyAdvertisement'),
  getOne: (id) => api.delete(`/GetAdvertisement/${id}`), // API uses DELETE method
  add: (data) => api.post('/AddAdvertisement', data),
  update: (data) => api.patch('/UpdateAdvertisement', data),
  delete: () => api.delete('/DeleteAdvertisement'),
  offer: (data) => api.put('/OfferToAdvertisement', data),
};

export default api;
