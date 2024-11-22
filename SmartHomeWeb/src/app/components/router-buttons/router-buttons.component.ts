import {Component, Input} from '@angular/core';
import {Router} from '@angular/router';

@Component({
  selector: 'app-router-buttons',
  templateUrl: './router-buttons.component.html',
  standalone: true,
  imports: [],
  styleUrls: ['./router-buttons.component.css']
})
export class RouterButtonsComponent {
  @Input() text: string = 'Click Me';
  @Input() color: string = '#007bff'; // Default tech website color
  @Input() url: string = '/';

  constructor(private router: Router) {
  }

  navigate() {
    this.router.navigate([this.url]);
  }
}
