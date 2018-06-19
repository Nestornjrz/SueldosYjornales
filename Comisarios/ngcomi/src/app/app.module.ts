import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EmpleadosComponent } from './empleados/empleados.component';
import { InicioComponent } from './inicio/inicio.component';
import { EmpDatosBasicosComponent } from 'src/app/empleados/emp-datos-basicos/emp-datos-basicos.component';
import {TabViewModule} from 'primeng/tabview';


@NgModule({
  declarations: [
    AppComponent,
    EmpleadosComponent,
    InicioComponent,
    EmpDatosBasicosComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule, FormsModule, TabViewModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
