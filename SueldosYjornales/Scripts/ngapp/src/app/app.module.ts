import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { AppRoutingModule } from './app-routing.module';

import {AccordionModule, SharedModule, CheckboxModule, DataTableModule, CalendarModule, MessagesModule} from 'primeng/primeng';

import { AppComponent } from './app.component';
import { EmpleadosService } from './services/empleados.service';
import { WindowsRefService } from './services/windows-ref.service';
import { ModificarPrestamoComponent } from './modificar-prestamo/modificar-prestamo.component';
import { ListEmpleadosComponent } from './modificar-prestamo/list-empleados/list-empleados.component';
import { DetPrestamosComponent } from './modificar-prestamo/det-prestamos/det-prestamos.component';
import { PrestamosSimplesService } from './services/prestamos-simples.service';
import { PrestamoIndividualComponent } from './modificar-prestamo/det-prestamos/prestamo-individual/prestamo-individual.component';


@NgModule({
  declarations: [
    AppComponent,
    ModificarPrestamoComponent,
    ListEmpleadosComponent,
    DetPrestamosComponent,
    PrestamoIndividualComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpModule,
    AppRoutingModule,
    AccordionModule, SharedModule, CheckboxModule, DataTableModule, CalendarModule, MessagesModule
  ],
  providers: [EmpleadosService, WindowsRefService, PrestamosSimplesService],
  bootstrap: [AppComponent]
})
export class AppModule { }
