import React from 'react';
import { Input, Select, Button, Space } from 'antd';
import IFolder from '../interfaces/IFolder';

const { TextArea } = Input;
const { Option } = Select;

class TaskCreationForm extends React.Component<ITaskCreationFormProps, ITaskCreationFormState> {
    constructor(props: ITaskCreationFormProps) {
        super (props);

        this.state = {
            title: '',
            description: '',
            folderId: 0,
        }
    }

    async createTask() {
        const { title, description, folderId } = this.state;
        await this.props.createTask(title, description, folderId);
        this.setState({ title: '', description: '', folderId: 0 });
    }

    onTitleChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.setState({ title: e.target.value });
    }

    onDescriptionChange(e: React.ChangeEvent<HTMLTextAreaElement>) {
        this.setState({ description: e.target.value });
    }

    onFolderChange(id: number) {
        this.setState({ folderId: id});
    }

    isButtonDisabled() {
        const { title, folderId } = this.state;
        return title.trim() === '' || folderId === 0;
    }

    render() {
        return (
            <Space direction='vertical'>
                <strong>Новая задача</strong>
                <Input
                    placeholder='Название задачи'
                    value={this.state.title}
                    onChange={e => this.onTitleChange(e)}
                    style={{ width: 300 }}
                />
                <TextArea
                    placeholder='Описание задачи'
                    value={this.state.description}
                    onChange={e => this.onDescriptionChange(e)}
                    style={{ width: 300 }}
                />
                <Select
                    placeholder='Папка'
                    onChange={id => this.onFolderChange(id)}
                    style={{ width: 300 }}
                >
                {
                    this.props.folders.map(f =>
                        <Option key={f.id} value={f.id}>{f.title}</Option>
                    )
                }
                </Select>
                <Button
                    onClick={() => this.createTask()}
                    disabled={this.isButtonDisabled()}
                >Добавить</Button>
            </Space>
        );
    }
}

export default TaskCreationForm;

interface ITaskCreationFormProps {
    folders: IFolder[];
    createTask: (title: string, description: string, folderId: number) => Promise<void>;
}

interface ITaskCreationFormState {
    title: string;
    description: string;
    folderId: number;
}
