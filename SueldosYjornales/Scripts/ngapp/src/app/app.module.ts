import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { AppRoutingModule } from './app-routing.module';

import {AccordionModule,SharedModule, CheckboxModule} from 'primeng/primeng';

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
    BrowserAnimationsModule,
    FormsModule,
    HttpModule,
    AppRoutingModule,
    AccordionModule, SharedModule, CheckboxModule
  ],
  providers: [EmpleadosService, WindowsRefService],
  bootstrap: [AppComponent]
})
export class AppModule { }
