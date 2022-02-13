import React from 'react';
import moment, { Moment } from 'moment';
import { Input, Select, DatePicker, Button, Space } from 'antd';
import IFolder from '../interfaces/IFolder';
import ITask from '../interfaces/ITask';

const { TextArea } = Input;
const { Option } = Select;

class TaskCreationForm extends React.Component<ITaskCreationFormProps, ITask> {
    constructor(props: ITaskCreationFormProps) {
        super (props);

        this.state = {
            id: null,
            title: '',
            description: '',
            dueDate: null,
            folderId: 0,
        }
    }

    async createTask() {
        await this.props.createTask(this.state);
        this.setState({ title: '', description: '', dueDate: null });
    }

    onTitleChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.setState({ title: e.target.value });
    }

    onDescriptionChange(e: React.ChangeEvent<HTMLTextAreaElement>) {
        this.setState({ description: e.target.value });
    }

    onDueDateChange(date: Moment | null) {
        const dueDate = date?.toDate() ?? null;
        this.setState({ dueDate });
    }

    onFolderChange(id: number) {
        this.setState({ folderId: id});
    }

    isButtonDisabled() {
        const { title, folderId } = this.state;
        return title.trim() === '' || folderId === 0;
    }

    render() {
        const dueDate = this.state.dueDate !== null
            ? moment(this.state.dueDate)
            : null;

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
                <DatePicker
                    placeholder='Дата'
                    value={dueDate}
                    onChange={date => this.onDueDateChange(date)}
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
    createTask: (task: ITask) => Promise<void>;
}
