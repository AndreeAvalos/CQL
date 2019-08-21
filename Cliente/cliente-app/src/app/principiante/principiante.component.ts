import { Component, OnInit } from '@angular/core';

declare var Blockly: any;
@Component({
  selector: 'app-principiante',
  templateUrl: './principiante.component.html',
  styleUrls: ['./principiante.component.scss']
})
export class PrincipianteComponent implements OnInit {
  title: string;
  programName: string;
  workspace: any;

  constructor(
  ) {}

  ngOnInit() {
    this.workspace = Blockly.inject('blocklyDiv', {
      toolbox: document.getElementById('toolbox'),
      scrollbars: false
    });

}
}
