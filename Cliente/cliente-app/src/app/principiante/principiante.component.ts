import { Component,OnInit } from '@angular/core';

declare var Blockly: any;


@Component({
  selector: 'app-principiante',
  templateUrl: './principiante.component.html',
  styleUrls: ['./principiante.component.scss'],
})

export class PrincipianteComponent implements OnInit {
  workspace: any;
  public data: any[] = [
    {
        text: 'DATABASES', items: [
            { text: 'DATABASE 1', items: [{text: 'TABLES' },{text: 'TYPES' },{text: 'PROCEDURES' }]},
            { text: 'DATABASE 2' },
            { text: 'DATABASE 3' }
        ]
    }
];

  constructor() {
  }

  ngOnInit() {
    this.workspace = Blockly.inject('blocklyDiv', {
      toolbox: document.getElementById('toolbox'),
      scrollbars: false
    });
  }
}
