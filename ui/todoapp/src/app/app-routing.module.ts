import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { TaskComponent } from './task/task.component';
import { TagComponent } from './tag/tag.component';
import { ReportComponent } from './report/report.component';

const routes: Routes = [
  {path: 'home', component: HomeComponent},
  {path: 'task', component: TaskComponent},
  {path: 'report', component: ReportComponent},
  {path: 'tag', component: TagComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
