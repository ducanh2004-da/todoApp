// src/app/services/task.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, forkJoin } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Task } from '../models/task.model';
import { Tag } from '../models/tag.model';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private readonly API_BASE = environment.API_URL.replace(/\/?$/, '/');
  private readonly taskUrl     = this.API_BASE + 'task';
  private readonly tagUrl      = this.API_BASE + 'tag';
  private readonly tagtaskUrl  = this.API_BASE + 'tagtask';
  private readonly exportUrl   = this.API_BASE + 'task/csvexport';

  constructor(private http: HttpClient) {}

  loadData(status: string): Observable<[Task[], Tag[]]> {
    const params = new HttpParams().set('status', status);
    return forkJoin([
      this.http.get<Task[]>(this.taskUrl, { params }),
      this.http.get<Tag[]>(this.tagUrl)
    ]);
  }

  getTaskById(id: number): Observable<Task> {
    return this.http.get<Task>(`${this.taskUrl}/${id}`);
  }

  createTask(data: any): Observable<void> {
    // data includes TaskTitle, Description, Priority, StartDay, EndDay, IsDone?, TagId
    return this.http.post<{ TaskId: number }>(this.taskUrl, data)
      .pipe(
        switchMap(res =>
          this.http.post<void>(this.tagtaskUrl, {
            TaskId: res.TaskId,
            TagId: data.TagId
          })
        )
      );
  }

  updateTask(data: any): Observable<void> {
    return this.http.put<void>(`${this.taskUrl}/${data.TaskId}`, data);
  }

  deleteTask(id: number): Observable<void> {
    return this.http.delete<void>(`${this.taskUrl}/${id}`);
  }

  completeTask(id: number): Observable<void> {
    return this.http.put<void>(`${this.taskUrl}/complete/${id}`, {});
  }

  exportCsv(status: string): Observable<Blob> {
    const params = new HttpParams().set('status', status);
    return this.http.get(this.exportUrl, { params, responseType: 'blob' });
  }
}
