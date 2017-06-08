import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { EmpleadosService } from './services/empleados.service';
import { WindowsRefService } from './services/windows-ref.service';
import { ModificarPrestamoComponent } from './modificar-prestamo/modificar-prestamo.component';
import { ListEmpleadosComponent } from './modificar-prestamo/list-empleados/list-empleados.component';

@NgModule({
  declarations: [
    AppComponent,
    ModificarPrestamoComponent,
    ListEmpleadosComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    AppRoutingModule
  ],
  providers: [EmpleadosService, WindowsRefService],
  bootstrap: [AppComponent]
})
export class AppModule { }
