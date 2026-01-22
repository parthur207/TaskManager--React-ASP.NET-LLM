import type { AuthResponse, LoginRequest } from "./types";

export function login(data: LoginRequest) {
  return http.post<AuthResponse>('/auth/login', data);
}