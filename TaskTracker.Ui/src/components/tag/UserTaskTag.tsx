import React from "react";
import { observer } from "mobx-react-lite";
import ITag from "../../interfaces/ITag";
import { Tag } from "antd";

interface IUserTaskTagProps {
    tag: ITag;
}

const UserTaskTag: React.FC<IUserTaskTagProps> = observer(props => {
    const { tag } = props;

    return (
        <Tag color={`#${tag.color}`}>
            <div style={{ mixBlendMode: "difference" }}>{tag.title}</div>
        </Tag>
    );
});

export default UserTaskTag;
