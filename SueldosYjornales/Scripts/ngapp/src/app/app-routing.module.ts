import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ModificarPrestamoComponent } from "app/modificar-prestamo/modificar-prestamo.component";

const routes: Routes = [
  { path: '', pathMatch:'full', redirectTo:'modificarPrestamo' },
  { path: 'modificarPrestamo', component: ModificarPrestamoComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
