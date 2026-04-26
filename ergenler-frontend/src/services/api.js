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
  getMdfStock: () => api.get('/Material/Stok/Mdf'),
  getGlueStock: () => api.get('/Material/Stok/Glue'),
  getBackPanelStock: () => api.get('/Material/Stok/BackPanel'),
  getScraps: () => api.get('/Material/Stok/Scraps'),
  getPvcBandStock: () => api.get('/Material/Stok/PvcBand'),

  // ADD stock
  addMdf: (data) => api.post('/Material/Stok/AddMdf', data),
  addGlue: (data) => api.post('/Material/Stok/AddGlue', data),
  addBackPanel: (data) => api.post('/Material/Stok/AddBackPanel', data),
  addPvcBand: (data) => api.post('/Material/Stok/AddPvcBand', data),
  addScrap: (data) => api.post('/Material/Stok/AddScrap', data),

  // UPDATE stock
  updateMdf: (id, data) => api.put(`/Material/Update/Mdf/${id}`, data),
  updateGlue: (id, data) => api.put(`/Material/Update/Glue/${id}`, data),
  updateBackPanel: (id, data) => api.put(`/Material/Update/BackPanel/${id}`, data),
  updatePvcBand: (id, data) => api.put(`/Material/Update/PvcBand/${id}`, data),
  updateScrap: (id, data) => api.put(`/Material/Update/Scrap/${id}`, data),

  // REDUCE stock
  reduceMdf: (data) => api.put('/Material/Stock/Reduce/Mdf', data),
  reduceGlue: (data) => api.put('/Material/Stock/Reduce/Glue', data),
  reducePvcBand: (data) => api.put('/Material/Stock/Reduce/PvcBand', data),
  reduceScrap: (data) => api.put('/Material/Stock/Reduce/Scrap', data),
  reduceBackPanel: (data) => api.put('/Material/Stock/Reduce/BackPanel', data),
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
  addInvoice: (data) => api.post('/Accounting/AddInvoice', data),
  payInvoice: (id) => api.post(`/Accounting/MarkThePayedToInvoice/${id}`),

  // Expenses
  getAllExpenses: () => api.get('/Accounting/GetAllExpense'),
  addExpense: (data) => api.post('/Accounting/AddExpense', data),
  updateExpense: (id, data) => api.patch(`/Accounting/UpdateExpense/${id}`, data),

  // Incomes
  getAllIncomes: () => api.get('/Accounting/GetAllIncome'),
  addIncome: (data) => api.post('/Accounting/AddIncome', data),
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
