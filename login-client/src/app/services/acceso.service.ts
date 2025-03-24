import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { appsettings } from '../settings/appsettings';
import { User } from '../interfaces/user';
import { Observable } from 'rxjs';
import { ResponseAcceso } from '../interfaces/response-acceso';
import { Login } from '../interfaces/login';

@Injectable({
  providedIn: 'root',
})
export class AccesoService {
  private http = inject(HttpClient);
  private baseUrl: string = appsettings.apiUrl;

  constructor() {}

  // registra usuario
  register(objeto: User): Observable<ResponseAcceso> {
    return this.http.post<ResponseAcceso>(
      `${this.baseUrl}Acceso/Registrarse`,
      objeto
    );
  }

  // Acceso de usuario
  login(objeto: Login): Observable<ResponseAcceso> {
    return this.http.post<ResponseAcceso>(
      `${this.baseUrl}Acceso/Login`,
      objeto
    );
  }

  validarToken(token: string): Observable<ResponseAcceso>{
    return this.http.get<ResponseAcceso>(`${this.baseUrl}Acceso/ValidarToken?token=${token}`)
  }

}
