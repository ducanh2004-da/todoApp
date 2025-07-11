import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { TaskComponent } from './task/task.component';
import { TagComponent } from './tag/tag.component';
import { ReportComponent } from './report/report.component';
import { FilterComponent } from './task/filter/filter.component';
import { TaskFormComponent } from './task/task-form/taskForm.component';

import { HttpClientModule } from '@angular/common/http';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
//material UI modules
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatToolbarModule } from '@angular/material/toolbar';  // <-- import đây
import { MatIconModule }    from '@angular/material/icon';
import { MatButtonModule }     from '@angular/material/button';
// RxWeb Reactive Forms validation
import { RxReactiveFormsModule } from '@rxweb/reactive-form-validators';
@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    TaskComponent,
    TagComponent,
    ReportComponent,
    FilterComponent,
    TaskFormComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatSlideToggleModule,
    // Angular Material modules cần cho datepicker + form-field + input + icon + button
    MatFormFieldModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    // RxWeb Reactive Forms validation
    RxReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
