import React from "react";
import { observer } from "mobx-react-lite";
import ITag from "../../interfaces/ITag";
import TagUpdateModal from "./TagUpdateModal";

interface ITagUpdateModalMenuItemProps {
    tag: ITag;
}

const TagUpdateModalMenuItem: React.FC<ITagUpdateModalMenuItemProps> = observer(
    props => {
        const [isModalOpen, setIsModalOpen] = React.useState(false);

        const handleUpdateTagButtonClick = () => {
            setIsModalOpen(true);
        };

        const hideTagUpdateModal = () => {
            setIsModalOpen(false);
        };

        return (
            <>
                <div onClick={handleUpdateTagButtonClick}>
                    Редактировать тег
                </div>
                <TagUpdateModal
                    tag={props.tag}
                    isModalOpen={isModalOpen}
                    hideModal={hideTagUpdateModal}
                />
            </>
        );
    }
);

export default TagUpdateModalMenuItem;
