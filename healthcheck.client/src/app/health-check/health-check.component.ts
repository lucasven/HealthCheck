import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Environment } from '../../environments/environment';

@Component({
  selector: 'app-health-check',
  templateUrl: './health-check.component.html',
  styleUrl: './health-check.component.scss'
})
export class HealthCheckComponent implements OnInit {
  public result?: Result;

  constructor(private http: HttpClient) {
  }

  ngOnInit() {
    this.http.get<Result>(`${Environment.baseUrl}api/health`).subscribe({
      next: result => {
        this.result = result;
      },
      error: error => console.error(error),
      complete: () => { }
    });
  }
}

interface Result {
  checks: Check[];
  totalStatus: string;
  totalResponseTime: number;
}

interface Check {
  name: string;
  status: string;
  responseTime: number;
  description: string;
}
