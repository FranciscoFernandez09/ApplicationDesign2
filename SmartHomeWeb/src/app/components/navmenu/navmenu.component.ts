import {Component, Input} from '@angular/core';
import {NgClass, NgForOf, NgIf} from '@angular/common';
import {Router} from '@angular/router';

@Component({
  selector: 'app-navmenu',
  templateUrl: './navmenu.component.html',
  standalone: true,
  imports: [NgForOf, NgIf, NgClass],
  styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {
  @Input() elements: {
    name: string;
    url: string;
    type: string;
  }[] = [];

  constructor(private router: Router) {
  }

  navigate(url: string) {
    this.router.navigate([url]);
  }

  isActive(url: string): boolean {
    return this.router.url === url;
  }
}
