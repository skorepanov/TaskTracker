import React from "react";
import { Input, Button } from 'antd';

class FolderCreationForm extends React.Component<IFolderCreationFormProps, IFolderCreationFormState> {
    constructor(props: IFolderCreationFormProps) {
        super(props);

        this.state = {
            title: ''
        };

        this.createTask = this.createTask.bind(this);
    }

    async createTask() {
        let title = this.state.title;
        await this.props.createFolder(title);
        this.setState({ title: '' });
    }

    onTitleChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.setState({ title: e.target.value });
    }

    isButtonDisabled() {
        return this.state.title.trim() === '';
    }

    render() {
        return (
            <div>
                <strong>Новая папка</strong><br />
                <Input
                    placeholder="Название папки"
                    value={this.state.title}
                    onChange={(e) => this.onTitleChange(e) }
                />
                <Button
                    onClick={this.createTask}
                    disabled={this.isButtonDisabled()}
                >Добавить</Button>
            </div>
        );
    }
}

export default FolderCreationForm;

interface IFolderCreationFormProps {
    createFolder: (title: string) => Promise<void>;
}

interface IFolderCreationFormState {
    title: string;
}
