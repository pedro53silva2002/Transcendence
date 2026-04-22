import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-confirmation-pop-up',
//   standalone: true,
  imports: [MatDialogModule, MatButtonModule],
  templateUrl: './confirmation-pop-up.html',
  styleUrl: './confirmation-pop-up.css',
})
export class ConfirmationPopUp {
	
	//onde as coisas são preparadas assim que surge o pop-up
	constructor(
		//serve para o pop-up se fechar a si próprio
		public dialogRef: MatDialogRef<ConfirmationPopUp>,
		//onde injetamos a informação título e mensagem
		@Inject(MAT_DIALOG_DATA) public data: { title: string; message: string }
	) {}

	onCancel(): void {
		this.dialogRef.close(false);
	}

	onConfirm(): void {
		this.dialogRef.close(true);
	}

}
