import { Q } from '@angular/cdk/keycodes';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatFormField } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { debounceTime, distinctUntilChanged, Subscription } from 'rxjs';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [MatButtonModule,
			MatMenuModule,
			ReactiveFormsModule,
			MatFormField,
			MatInputModule,
			MatAutocompleteModule,
			RouterLink,
			RouterLinkActive,
			MatIconModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent implements OnInit {
	searchControl = new FormControl('');
	users = ['Maria', 'João', 'Diogo', 'Maria João', 'Rui Diogo', 'Rui'];
	filteredUsers: string[] = [];
	private searchSub?: Subscription;

	// constructor() {
	// 	this.searchControl.valueChanges.subscribe(value => {
	// 		console.log("Current search query:", value);
	// 	})
	// }

	ngOnInit(): void {
		this.searchSub = this.searchControl.valueChanges.pipe(
			debounceTime(300),
			distinctUntilChanged(),
		).subscribe(query => {
			console.log(query)
			if(query!= null){
				this.filteredUsers= this.users.filter(user => user.toLowerCase().includes(query.toLowerCase()))
				console.log(this.filteredUsers)
			}
		})
	}
}
