import React from 'react';
import dayjs, { Dayjs } from 'dayjs';
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
            completedDateTime: null,
            folderId: null,
            dueDateTime: null,
        }
    }

    createTask = async () => {
        await this.props.createTask(this.state);

        this.setState({
            title: '',
            description: '',
            folderId: null,
            dueDateTime: null
        });
    }

    onTitleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({ title: e.target.value });
    }

    onDescriptionChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        this.setState({ description: e.target.value });
    }

    onDueDateTimeChange = (date: Dayjs | null) => {
        const dueDateTime = date?.toDate() ?? null;
        this.setState({ dueDateTime: dueDateTime });
    }

    onFolderChange = (id: number) => {
        this.setState({ folderId: id});
    }

    isButtonDisabled = () => {
        const { title } = this.state;
        return title.trim() === '';
    }

    render() {
        const dueDateTime = this.state.dueDateTime !== null
            ? dayjs(this.state.dueDateTime)
            : null;

        return (
            <Space direction='vertical'>
                <strong>Новая задача</strong>
                <Input
                    placeholder='Название задачи'
                    value={this.state.title}
                    onChange={this.onTitleChange}
                    style={{ width: 300 }}
                />
                <TextArea
                    placeholder='Описание задачи'
                    value={this.state.description}
                    onChange={this.onDescriptionChange}
                    style={{ width: 300 }}
                />
                <DatePicker
                    placeholder='Дата'
                    value={dueDateTime}
                    onChange={this.onDueDateTimeChange}
                />
                <Select
                    placeholder='Папка'
                    onChange={this.onFolderChange}
                    style={{ width: 300 }}
                >
                {
                    this.props.folders.map(f =>
                        <Option
                            key={f.id}
                            value={f.id}
                        >
                            {f.title}
                        </Option>
                    )
                }
                </Select>
                <Button
                    onClick={this.createTask}
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
