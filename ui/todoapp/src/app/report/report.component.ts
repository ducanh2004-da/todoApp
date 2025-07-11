import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';

interface Report {
  TotalTasks: number;
  DoneTasks: number;
  PendingTasks: number;
  HighPriority: number;
  MediumPriority: number;
  LowPriority: number;
}
@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements OnInit {

  reports!: Report;
  loading = false;
  error = '';

  private readonly baseUrl = environment.API_URL.endsWith('/')
    ? environment.API_URL + 'task/report'
    : environment.API_URL + '/task/report';

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.refreshList();
  }

  refreshList() {
    this.http.get<Report>(this.baseUrl)
      .subscribe((data) => 
        {
          this.reports = data;
          console.log('Reports fetched successfully:', this.reports);
        },
        error => {
          console.error('Error fetching tasks:', error);
          alert('Failed to load tasks. Please try again later.');
        }
      )
  }

}
