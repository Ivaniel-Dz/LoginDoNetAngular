import { Component, inject } from '@angular/core';
import { ProductoService } from '../../services/producto.service';
import { Producto } from '../../interfaces/producto';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-inicio',
  imports: [MatCardModule, MatTableModule],
  templateUrl: './inicio.component.html',
  styleUrl: './inicio.component.css',
})
export class InicioComponent {
  private productoService = inject(ProductoService);
  public listaProducto: Producto[] = [];
  public displayedColumns: string[] = ['nombre', 'marca', 'precio'];

  constructor() {
    this.productoService.lista().subscribe({
      next: (data) => {
        if (data.value.length > 0) {
          this.listaProducto = data.value;
        }
      },
      error: (err) => {
        console.log(err.message);
      },
    });
  }
}
