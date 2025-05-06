import React from "react";
import { Input, Button, Space } from 'antd';

class FolderCreationForm extends React.Component<IFolderCreationFormProps, IFolderCreationFormState> {
    constructor(props: IFolderCreationFormProps) {
        super(props);

        this.state = {
            title: ''
        };
    }

    createFolder = async () => {
        const { title } = this.state;
        await this.props.createFolder(title);
        this.setState({ title: '' });
    }

    onTitleChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({ title: e.target.value });
    }

    isButtonDisabled() {
        return this.state.title.trim() === '';
    }

    render() {
        return (
            <Space direction='vertical'>
                <strong>Новая папка</strong>
                <Input
                    placeholder='Название папки'
                    value={this.state.title}
                    onChange={this.onTitleChange}
                    style={{ width: 300 }}
                />
                <Button
                    onClick={this.createFolder}
                    disabled={this.isButtonDisabled()}
                >Добавить</Button>
            </Space>
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
