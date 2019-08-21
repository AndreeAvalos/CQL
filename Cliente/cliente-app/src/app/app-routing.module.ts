import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PrincipianteComponent } from './principiante/principiante.component';
import { LoginComponent } from './login/login.component';
import { IntermedioComponent } from './intermedio/intermedio.component';
import { AvanzadoComponent } from './avanzado/avanzado.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  { path: 'Principiante', component: PrincipianteComponent },
  { path: 'Intermedio', component: IntermedioComponent },
  { path: 'Avanzado', component: AvanzadoComponent },
  { path: 'home', component: HomeComponent },
  { path: 'Login', component: LoginComponent },
  { path: '', redirectTo: '/Login', pathMatch: 'full' },
  { path: '**', redirectTo: '/Login', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
