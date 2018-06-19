import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CalendarModule } from 'primeng/calendar';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EmpleadosComponent } from './empleados/empleados.component';
import { InicioComponent } from './inicio/inicio.component';
import { EmpDatosBasicosComponent } from 'src/app/empleados/emp-datos-basicos/emp-datos-basicos.component';
import { TabViewModule } from 'primeng/tabview';
import { NgPrimeCustomService } from 'src/app/services/ng-prime-custom.service';


@NgModule({
  declarations: [
    AppComponent,
    EmpleadosComponent,
    InicioComponent,
    EmpDatosBasicosComponent
  ],
  imports: [
    BrowserModule, BrowserAnimationsModule,
    AppRoutingModule, FormsModule, TabViewModule, CalendarModule
  ],
  providers: [NgPrimeCustomService],
  bootstrap: [AppComponent]
})
export class AppModule { }
