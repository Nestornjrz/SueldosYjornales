import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { EmpleadosService } from './services/empleados.service';
import { WindowsRefService } from './services/windows-ref.service';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule
  ],
  providers: [EmpleadosService, WindowsRefService],
  bootstrap: [AppComponent]
})
export class AppModule { }