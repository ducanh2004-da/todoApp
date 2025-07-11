import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

// mẫu để định nghĩa dữ liệu truyền vào cho edit
interface Tag {
  TagId: number;
  TagName: string;
  Color: string;
  TaskList: string[];
}
@Component({
  selector: 'app-tag',
  templateUrl: './tag.component.html',
  styleUrls: ['./tag.component.css']
})
export class TagComponent implements OnInit {

  // danh sách các biến để thao tác lên giao diện
  tags: Tag[] = [];
  modalTitle = '';
  TagId = 0;
  TagName = '';
  Color = '';
  TaskList: string[] = [];

  // baseUrl đảm bảo URL API luôn đúng định dạng
  private readonly baseUrl = environment.API_URL.endsWith('/')
    ? environment.API_URL + 'tag'
    : environment.API_URL + '/tag';

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.refreshList();
  }
  refreshList() {
    this.http.get<Tag[]>(this.baseUrl)
      .subscribe(data => this.tags = data,
        error => {
          console.error('Error fetching tasks:', error);
          alert('Failed to load tasks. Please try again later.');
        }
      )
  }

  addClick() {
    this.modalTitle = 'Add Tag';
    this.TagId = 0;
    this.TagName = '';
    this.Color = '';
    this.TaskList = [];
  }
  editClick(tag: Tag) {
    this.modalTitle = 'Edit Tag';
    this.TagId = tag.TagId;
    this.TagName = tag.TagName;
    this.Color = tag.Color;
    this.TaskList = tag.TaskList;
  }
  createClick() {
    const val = {
      TagName: this.TagName,
      Color: this.Color,
      TaskList: this.TaskList
    }
    this.http.post(this.baseUrl, val)
      .subscribe(() => {
        alert('Tag created successfully');
        this.refreshList();
      }, error => {
        console.error('Error creating tag:', error);
        alert('Failed to create tag. Please try again later.');
      });
  }
  updateClick() {
    const val = {
      TagId: this.TagId,
      TagName: this.TagName,
      Color: this.Color,
      TaskList: this.TaskList
    }
    this.http.put(`${this.baseUrl}/${this.TagId}`, val)
      .subscribe(() => {
        alert('Tag updated successfully');
        this.refreshList();
      }, error => {
        console.error('Error updating tag:', error);
        alert('Failed to update tag. Please try again later.');
      });
  }
  deleteClick(id: number) {
    if (!confirm('Are you sure you want to delete this tag?')) return;
      this.http.delete(`${this.baseUrl}/${id}`)
        .subscribe(() => {
          alert('Tag deleted successfully');
          this.refreshList();
        }, error => {
          console.error('Error deleting tag:', error);
          alert('Failed to delete tag. Please try again later.');
        });
  }


}
