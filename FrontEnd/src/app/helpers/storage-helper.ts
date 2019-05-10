export function getAuthToken() {
  return localStorage.getItem('auth_token') || sessionStorage.getItem('auth_token');
}

export function setAuthToken(token: string, remember: boolean) {
  clearAuthToken();

  const storage = remember ? localStorage : sessionStorage;
  storage.setItem('auth_token', token);
}

export function clearAuthToken() {
  localStorage.removeItem('auth_token');
  sessionStorage.removeItem('auth_token');
}
