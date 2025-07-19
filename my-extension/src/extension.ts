import * as vscode from 'vscode';

export function activate(context: vscode.ExtensionContext) {

	console.log('Congratulations, your extension "my-extension" is now active!');

	const disposable = vscode.commands.registerCommand('my-extension.helloWorld', () => {
		vscode.window.showInformationMessage('Hello World from my-extension!');
	});

	context.subscriptions.push(disposable);
}

export function deactivate() {}
