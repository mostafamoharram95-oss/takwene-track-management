import { Component } from '@angular/core';
import { inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TrackApi } from '../../services/track-api';
import { AsyncPipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  imports: [AsyncPipe, DatePipe, RouterLink],
  selector: 'app-track-detail',
  styleUrl: './track-detail.css',
  templateUrl: './track-detail.html',
})
export class TrackDetail {
  private readonly route = inject(ActivatedRoute);
  private readonly trackApi = inject(TrackApi);
  trackId = Number(this.route.snapshot.paramMap.get('id'));
  track$ = this.trackApi.getTrackById(this.trackId);
}
