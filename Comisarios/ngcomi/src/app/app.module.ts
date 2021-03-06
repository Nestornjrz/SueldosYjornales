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
import { WindowsRefService } from 'src/app/services/windows-ref.service';
import { HttpClientService } from 'src/app/services/http-client.service';
import { HttpClientModule } from '@angular/common/http';
import { EmpListDatosBasicosComponent } from './empleados/emp-list-datos-basicos/emp-list-datos-basicos.component';


@NgModule({
  declarations: [
    AppComponent,
    EmpleadosComponent,
    InicioComponent,
    EmpDatosBasicosComponent,
    EmpListDatosBasicosComponent
  ],
  imports: [
    BrowserModule, BrowserAnimationsModule,
    AppRoutingModule, FormsModule, HttpClientModule, TabViewModule, CalendarModule
  ],
  providers: [WindowsRefService, NgPrimeCustomService, HttpClientService],
  bootstrap: [AppComponent]
})
export class AppModule { }
