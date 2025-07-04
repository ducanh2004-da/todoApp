import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

interface Task {
  TaskId: number;
  TaskTitle: string;
  Description: string;
  Priority: number;
  StartDay: Date;
  EndDay: Date;
  IsDone: boolean;
  TagList: string[];
}

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})
export class TaskComponent implements OnInit {

  // danh sách các biến để thao tác lên giao diện
  tasks: Task[] = [];
  modalTitle = '';
  TaskId = 0;
  TaskTitle = '';
  Description = '';
  Priority = 0;
  StartDay = '';
  EndDay = '';
  IsDone = false;
  TagList: string[] = [];

  // baseUrl đảm bảo URL API luôn đúng định dạng
  private readonly baseUrl = environment.API_URL.endsWith('/')
    ? environment.API_URL + 'task'
    : environment.API_URL + '/task';
    
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.refreshList();
  }

  refreshList() {
    this.http.get<Task[]>(this.baseUrl)
      .subscribe(data => this.tasks = data,
        error => {
          console.error('Error fetching tasks:', error);
          alert('Failed to load tasks. Please try again later.');
        }
      )
  }

  addClick() {
    this.modalTitle = 'Add Task';
    this.TaskId = 0;
    this.TaskTitle = '';
    this.Description = '';
    this.Priority = 0;
    this.StartDay = '';
    this.EndDay = '';
    this.IsDone = false;
    this.TagList = [];
  }
  editClick(task: Task) {
    this.modalTitle = 'Edit Task';
    this.TaskId = task.TaskId;
    this.TaskTitle = task.TaskTitle;
    this.Description = task.Description;
    this.Priority = task.Priority;
    this.StartDay = task.StartDay.toString();
    this.EndDay = task.EndDay.toString();
    this.IsDone = task.IsDone;
    this.TagList = task.TagList;
  }
  createClick() {
    const val = {
      TaskId: this.TaskId,
      TaskTitle: this.TaskTitle,
      Description: this.Description,
      Priority: this.Priority,
      StartDay: this.StartDay,
      EndDay: this.EndDay,
    }
    this.http.post(this.baseUrl, val)
      .subscribe(() => {
        alert('Thêm công việc thành công');
        this.refreshList();
      },
        error => {
          console.error('Error creating task:', error);
          alert('Failed to create task. Please try again later.');
        }
      );
  }
  updateClick() {
    const val = {
      TaskId: this.TaskId,
      TaskTitle: this.TaskTitle,
      Description: this.Description,
      Priority: this.Priority,
      StartDay: this.StartDay,
      EndDay: this.EndDay,
      IsDone: this.IsDone,
      TagList: this.TagList
    }
    this.http.put(`${this.baseUrl}/${this.TaskId}`, val)
      .subscribe(() => {
        alert('Cập nhật công việc thành công');
        this.refreshList();
      },
        error => {
          console.error('Error updating task:', error);
          alert('Failed to update task. Please try again later.');
        }
      );
  }
  deleteClick(id:number){
    if(!confirm('Bạn có chắc chắn muốn xóa công việc này?')) {
      return;
    }
    this.http.delete(`${this.baseUrl}/${id}`)
      .subscribe(() => {
        alert('Xóa công việc thành công');
        this.refreshList();
      },
        error => {
          console.error('Error deleting task:', error);
          alert('Failed to delete task. Please try again later.');
        }
      );
  }


}
