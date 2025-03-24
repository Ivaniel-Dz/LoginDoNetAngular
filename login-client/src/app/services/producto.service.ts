import { inject, Injectable } from '@angular/core';
import { appsettings } from '../settings/appsettings';
import { HttpClient } from '@angular/common/http';
import { ResponseProducto } from '../interfaces/response-producto';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProductoService {
  private http = inject(HttpClient);
  private baseUrl: string = appsettings.apiUrl;

  constructor() {}

    // Muestra lista de productos
    lista(): Observable<ResponseProducto> {
      return this.http.get<ResponseProducto>(
        `${this.baseUrl}Producto/Lista`
      );
    }

}
