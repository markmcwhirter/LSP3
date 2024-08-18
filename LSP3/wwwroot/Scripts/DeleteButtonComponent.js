export class DeleteButtonComponent {
    eGui;
    eButton;
    eventListener;

    init() {
        this.eGui = document.createElement('div');
        let eButton = document.createElement('button');
        eButton.className = 'btn-simple';
        eButton.textContent = 'Click Me!';
        this.eventListener = () => alert('Button Clicked!');
        eButton.addEventListener('click', this.eventListener);
        this.eGui.appendChild(eButton);
    }

    getGui() {
        return this.eGui;
    }

    refresh() {
        return true;
    }

    destroy() {
        if (this.eButton) {
            this.eButton.removeEventListener('click', this.eventListener);
        }
    }
}