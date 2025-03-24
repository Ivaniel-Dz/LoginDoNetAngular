import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegistroComponent } from './pages/registro/registro.component';
import { InicioComponent } from './pages/inicio/inicio.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
    {path: "login", component:LoginComponent ,title: "Login"},
    {path: "registro", component:RegistroComponent, title: "Registro"},
    {path: "inicio", component:InicioComponent, title: "Inicio", canActivate:[authGuard]},
    {path: "**", redirectTo: 'login'}
];
