import { Component } from '@angular/core';
import { inject } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { TrackApi } from '../../services/track-api';
import {CommonModule} from "@angular/common";
import {RouterLink} from "@angular/router";

@Component({
  imports: [CommonModule, AsyncPipe, RouterLink],
  selector: 'app-track-list',
  styleUrl: './track-list.css',
  templateUrl: './track-list.html',
})
export class TrackList {
  private readonly trackApi = inject(TrackApi);
  tracks$ = this.trackApi.getTracks();

  onStatusChange(status: string) {
    this.tracks$ = this.trackApi.getTracks(status);
  }
}
